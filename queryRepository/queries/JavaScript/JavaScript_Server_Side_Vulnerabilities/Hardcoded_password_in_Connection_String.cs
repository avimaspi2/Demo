CxList allInDbFiles = NodeJS_Get_All_In_DB();

CxList onlyParams = allInDbFiles.FindByType(typeof (Param));

CxList sqlQuerymysql = 
	allInDbFiles.FindByShortNames(
	new List<string> {
		"createConnection",
		"Database"
	}
);

CxList connParam = onlyParams.GetParameters(sqlQuerymysql);	//parameter is of type anonySomeHash

connParam.Add(All.FindAllReferences( All.FindByFathers(connParam)).GetAssigner());

CxList allInConnMeth =  allInDbFiles.GetByAncs(connParam);

CxList allPasswords = NodeJS_Find_All_Passwords() * allInConnMeth;
CxList allStrings = NodeJS_Find_Strings() * allInConnMeth;
CxList passwordsInLeft = allPasswords.FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList stringsInRight = allStrings.FindByAssignmentSide(CxList.AssignmentSide.Right);

//find passwords in connection strings for mysql and db-mysql DBs
CxList assignToPass = passwordsInLeft.GetFathers();
CxList hdString = stringsInRight.GetByAncs(assignToPass);

result.Add(hdString);