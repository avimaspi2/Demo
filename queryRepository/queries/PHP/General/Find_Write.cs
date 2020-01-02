CxList methods = Find_Methods();
CxList errLogMethod = methods.FindByShortName("error_log");
	
//error_log with 0 or 3 error type
CxList errLogResults = errLogMethod.FindByParameterValue(1, "1", BinaryOperator.IdentityEquality)
	+ errLogMethod.FindByParameterValue(1, "2", BinaryOperator.IdentityEquality);
CxList errLog = errLogMethod - errLogResults;

result = Find_File_Write() + errLog;