// Find database and file accesses
CxList dbIn = Find_DB_In();
CxList dataAccess = Find_DB();
dataAccess.Add(Find_IO());
dataAccess.Add(Find_FileSystem_Read());
dataAccess.Add(Find_FileSystem_Write());

CxList methods = Find_Methods();

CxList suspectConditionParams = Find_UnknownReference();
suspectConditionParams.Add(Find_MemberAccess());
suspectConditionParams.Add(methods);
CxList resultSet = All.FindByMemberAccess("ResultSet.*");

// remove ResultSet.close()
CxList toRemove = resultSet.FindByShortName("close");
// remove results like ResultSet.CONCUR_READ_ONLY, etc
toRemove.Add(resultSet.FindByType(typeof(MemberAccess)));
toRemove.Add(dataAccess.FindByMemberAccess("System.getProperty"));
toRemove.Add(dataAccess.FindByMemberAccess("CallableStatement.get*"));
toRemove.Add(dataAccess.FindByMemberAccess("CallableStatement.set*"));
toRemove.Add(dataAccess.FindByMemberAccess("HttpServletRequest.get*"));
toRemove.Add(dataAccess.FindByMemberAccess("SQLResultSetReader.get*"));
toRemove.Add(dataAccess.FindByMemberAccess("ResultSet.wasNull"));
toRemove.Add(dataAccess.FindByMemberAccess("ResultSet.next"));
toRemove.Add(dataAccess.FindByMemberAccess("ResultSet.get*"));
toRemove.Add(dataAccess.FindByMemberAccess("JSPWriter.print*"));
//Use FindByShortName because getEnv found twice , 
//once as MemberAccess and once as MethodRef
toRemove.Add(dataAccess.FindByShortName("getenv"));

dataAccess -= toRemove;

//remove non-db methods influenced by HttpServletResponse because they are not considered a sensitive resource (e.g. println)
CxList httpServletResponse = All.FindByTypes(new String[]{"HttpServletResponse","ServletResponse"});
CxList dataAccessParameters = All.GetParameters(dataAccess);
CxList dataAccessInfluencedByServlet = All.NewCxList();


int limit = 30000;
// check the size of sink and source of influencing calculation
if ((dataAccess.Count > limit) && (httpServletResponse.Count > limit))
{
	// select only sink that appears in source file and wice versa
	CxList tempDataAccess = dataAccess.FindByFiles(httpServletResponse);
	CxList temphttpServletResponse = dataAccess.FindByFiles(tempDataAccess);

	// select size of the loop
	CxList small = All.NewCxList();
	CxList big = All.NewCxList();
	bool forward = true;
	if (tempDataAccess.Count > temphttpServletResponse.Count)
	{
		small.Add(temphttpServletResponse);
		big.Add(tempDataAccess);
		forward = false;
	}
	else
	{
		small.Add(tempDataAccess);
		big.Add(temphttpServletResponse);
		forward = true;
	}

	int count = 0;

	// create dictionary that includes source or sink sorted by file name (value). Key is fileId of this file 
	Dictionary<int,CxList> smallDic = new Dictionary<int,CxList>();

	foreach (CxList tmp in small)
	{
		CSharpGraph sg = tmp.GetFirstGraph();
		int fileId = sg.LinePragma.GetFileId();
		CxList cxList;
		if (!smallDic.TryGetValue(fileId, out cxList))
		{
			cxList = All.NewCxList();
			smallDic[fileId] = cxList;
		}
		cxList.Add(tmp);
		smallDic[fileId] = cxList;
	}


	// create dictionary that includes source or sink sorted by file name (value). Key is fileId of this file 
	Dictionary<int,CxList> bigDic = new Dictionary<int,CxList>();
	DateTime buildBig = DateTime.Now;
	foreach (CxList tmp1 in big)
	{
		CSharpGraph sg1 = tmp1.GetFirstGraph();
		int fileId1 = sg1.LinePragma.GetFileId();
		CxList cxList1;
		if (!bigDic.TryGetValue(fileId1, out cxList1))
		{
			cxList1 = All.NewCxList();
			bigDic[fileId1] = cxList1;
		}
		cxList1.Add(tmp1);
		bigDic[fileId1] = cxList1;
	}
	TimeSpan buildBigTs = DateTime.Now - buildBig;
	
	// implement searching of influencing algorithm for each file separatly
	foreach (int keyValue in smallDic.Keys)
	{
		CxList tempSmall = smallDic[keyValue];
		CxList tempBig;
	
		if (!bigDic.TryGetValue(keyValue, out tempBig))
		{
			continue;
		}
	
		CxList tempRes;
		if (forward)
			tempRes = tempSmall.InfluencedByAndNotSanitized(tempBig, dataAccessParameters);
		else
			tempRes = tempBig.InfluencedByAndNotSanitized(tempSmall, dataAccessParameters);
		dataAccessInfluencedByServlet.Add(tempRes.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly));

		count++;
	}
}
else
{
	dataAccessInfluencedByServlet = dataAccess.InfluencedByAndNotSanitized(httpServletResponse, dataAccessParameters);
	dataAccessInfluencedByServlet = dataAccessInfluencedByServlet.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
}


dataAccess -= dataAccessInfluencedByServlet;


//Find unatuthenticated authorization of requests from http security from spring framework 
CxList authorizeRequests = All.FindByMemberAccess("HttpSecurity.authorizeRequests");

CxList authenticated = authorizeRequests.GetRightmostMember().FindByShortName("authenticated");
CxList anyRequestAuthenticated = authenticated.GetTargetOfMembers().FindByShortName("anyRequest");
authorizeRequests -= anyRequestAuthenticated.GetLeftmostTarget().GetMembersOfTarget();
dataAccess.Add(authorizeRequests);

// Find conditions that make use of *auth* and *admin* words //- heuristics
CxList conditions = suspectConditionParams.GetByAncs(Find_Conditions());
List<string> possible_str = new List<string>(){"*admin*", "*allow",
		"*allowed", "*allows", "*deny", "*denies", "*denied",
		"*authoriz*", "*permission*"};
CxList potential_check = conditions.FindByShortNames(possible_str, false);
CxList ifStmtCheck = potential_check.GetAncOfType(typeof(IfStmt));

// All data accesses that contain permissions to do the access
CxList dataAccessCheck = dataAccess.GetByAncs(ifStmtCheck);

result = dataAccess - dataAccessCheck;
result -= Find_Properties_Files();