CxList methodInvoke = Find_Methods(); 
CxList onlyParams = Find_Parameters();

CxList output = All.NewCxList();

CxList allRequire = methodInvoke.FindByShortName("require");
CxList requireLibrary = onlyParams.GetParameters(allRequire, 0);

////////////////////////////////////////////////////////////////////////////////////////////////////
//add input mehtods for nano DB
CxList _params = requireLibrary.FindByShortName("\"nano\"", false);

CxList requireOfDBNano = allRequire.FindByParameters(_params);

List<String> allFilesWithDBNano = new List<String>();
CxList allINSQLNano = All.NewCxList();
foreach(CxList reqDB in requireOfDBNano)
{
	try
	{
		CSharpGraph reqDBGR = reqDB.GetFirstGraph();
		String rDBName = reqDBGR.LinePragma.FileName;
		if (!allFilesWithDBNano.Contains(rDBName))
		{
			allFilesWithDBNano.Add(rDBName);
			allINSQLNano.Add(All.FindByFileName(rDBName));
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}

CxList methInvSQLNano = allINSQLNano.FindByType(typeof (MethodInvokeExpr));

CxList sqlQueryNano = methInvSQLNano.FindByMemberAccess("*.get");
sqlQueryNano.Add(methInvSQLNano.FindByMemberAccess("*.head"));
sqlQueryNano.Add(methInvSQLNano.FindByMemberAccess("*.list"));
sqlQueryNano.Add(methInvSQLNano.FindByMemberAccess("*.fetch"));
sqlQueryNano.Add(methInvSQLNano.FindByMemberAccess("*.fetch_revs"));

output.Add(sqlQueryNano);
////////////////////////////////////////////////////////////////////////////////////////////////////
//add input mehtods for node-couchdb DB
CxList _params2 = requireLibrary.FindByShortName("\"node-couchdb\"", false);

CxList requireOfDBCouchdb = allRequire.FindByParameters(_params2);

List<String> allFilesWithDBcouchdb = new List<String>();
CxList allINSQLcouchdb = All.NewCxList();
foreach(CxList reqDB in requireOfDBCouchdb)
{
	try
	{
		CSharpGraph reqDBGR = reqDB.GetFirstGraph();
		String rDBName = reqDBGR.LinePragma.FileName;
		if (!allFilesWithDBcouchdb.Contains(rDBName))
		{
			allFilesWithDBcouchdb.Add(rDBName);
			allINSQLcouchdb.Add(All.FindByFileName(rDBName));
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}

CxList methInvSQLcouchdb = allINSQLcouchdb.FindByType(typeof (MethodInvokeExpr));

CxList sqlQueryCouchdb = methInvSQLcouchdb.FindByMemberAccess("*.get");

output.Add(sqlQueryCouchdb);
////////////////////////////////////////////////////////////////////////////////////////////////////
//add input mehtods for mongodb DB
CxList _params3 = requireLibrary.FindByShortName("\"mongodb\"", false);

CxList requireOfDBMongo = allRequire.FindByParameters(_params3);

List<String> allFilesWithDBMongo = new List<String>();
CxList allINSQLMongo = All.NewCxList();
foreach(CxList reqDB in requireOfDBMongo)
{
	try
	{
		CSharpGraph reqDBGR = reqDB.GetFirstGraph();
		String rDBName = reqDBGR.LinePragma.FileName;
		if (!allFilesWithDBMongo.Contains(rDBName))
		{
			allFilesWithDBMongo.Add(rDBName);
			allINSQLMongo.Add(All.FindByFileName(rDBName));
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}

CxList methInvSQLMongo = allINSQLMongo.FindByType(typeof (MethodInvokeExpr));
//only query() methods in files that include some DB
CxList sqlQueryMongo = methInvSQLMongo.FindByMemberAccess("*.find");

output.Add(sqlQueryMongo);
////////////////////////////////////////////////////////////////////////////////////////////////////


result = output;
result.Add(methodInvoke.FindByShortName("select"));

// Check if MOngoDB is used in this project
if (Find_Require("mongodb").Count > 0)
{
	// Find MongoDB by heuristics:
	CxList dbObjects = All.FindByShortName("db");
	CxList collections = dbObjects.GetMembersOfTarget().FindByShortNames(new List<string>{"collection", "allocations", "users",
			"contributions", "counters", "getCollection"});
	
	CxList assignRight = collections.FindByAssignmentSide(CxList.AssignmentSide.Right);
	CxList uRef = Find_UnknownReference();
	CxList decl = Find_Declarators();
	CxList assignLeft = Find_Assign_Lefts();
	CxList collectionAssigned = assignLeft.FindByFathers(assignRight.GetFathers());
	CxList unknownAssign = collectionAssigned.FindByType(typeof(UnknownReference));
	foreach (CxList reference in  unknownAssign)
	{
		try{
			CSharpGraph g = reference.GetFirstGraph();
			if(g != null && g.LinePragma != null && g.ShortName != null)
			{
				collectionAssigned.Add(decl.FindByFileId(g.LinePragma.GetFileId()).FindByPosition(g.LinePragma.Line, g.LinePragma.Column));
			}
		}catch(Exception e)
		{
			cxLog.WriteDebugMessage(e);
		}
	
	}
	collections.Add(uRef.FindAllReferences(collectionAssigned));
	List<string> findMethodsNames = new List<string>{"find", "findOne", "findAndModify", "get*"};
	CxList findMethods = collections.GetMembersOfTarget().FindByShortNames(findMethodsNames);
	CxList findParam = All.GetParameters(findMethods);

	// Fetches second parameter of anon functions, as first one is error object (function(err, *doc*){})
	CxList lambdaParams = findParam.FindByType(typeof(LambdaExpr));
	CxList secondParam = All.GetParameters(lambdaParams, 1);
	result.Add(findMethods);
	result.Add(secondParam);
}