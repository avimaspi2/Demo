CxList methodInvokes = Find_Methods();
List<string> sqlExecutionMethods 
	= new List<string>(){"EXECUTE_IMMEDIATE", "INSERT", "SELECT", "UPDATE", "EXEC_SQL", "EXECUTE_SQL"};
CxList executeMethods = methodInvokes.FindByShortNames(sqlExecutionMethods);
result = executeMethods;