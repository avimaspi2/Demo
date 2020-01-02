if (CGI().Count > 0){
	CxList db = Find_DB_Out();
	CxList methods = Find_Methods();
	
	CxList read = Find_Read_NonDB();
	read.Add(Find_FileSystem_Read());
	
	CxList outputs = Find_Console_Outputs();
	
	CxList sanitize = Find_XSS_Sanitize();
	sanitize -= read;
	sanitize.Add(Find_Integers());
	
	sanitize.Add(methods.FindByMemberAccess("System.getProperty"));
	//it was added here and not inside the sanitizers because read is removing it again and there is no intention to change 
	//general query
	CxList dbRead = All.NewCxList();
	dbRead.Add(db);
	dbRead.Add(read);
	
	result = dbRead.InfluencingOnAndNotSanitized(outputs, sanitize);
	result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);
}