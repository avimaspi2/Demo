CxList allMethods = Find_Methods();
CxList pgParamSanitizer = allMethods.FindByShortNames(new List<string>
	{"pg_execute", "pg_send_execute", "pg_convert", "pg_escape_string",	"pg_escape_bytea"});
	
//	allMethods.FindByShortName("pg_execute") +
//	allMethods.FindByShortName("pg_send_execute") +
//	allMethods.FindByShortName("pg_convert") +
//	allMethods.FindByShortName("pg_escape_string") +
//	allMethods.FindByShortName("pg_escape_bytea");
result.Add(pgParamSanitizer);

// pg_send_execute - SQL stmt params are sanitized.
CxList sendExecuteCalls = 
	allMethods.FindByShortName("pg_send_query_params");

CxList sendExecuteParam2 = 
	All.GetParameters(sendExecuteCalls, 2);

result.Add(sendExecuteParam2);

// pg_execute, pg_query_params
CxList varParamsExecCalls = 
	allMethods.FindByShortName("pg_query_params");

// Get the last parameter of each call - it is the parameters' array for the query.
foreach (CxList curExec in varParamsExecCalls)
{
	MethodInvokeExpr method = curExec.data.GetByIndex(0) as MethodInvokeExpr;
	if (method != null && method.Parameters != null && method.Parameters.Count > 0)
	{
		Param lastParam = method.Parameters[method.Parameters.Count - 1];
		Expression exp = lastParam.Value;		
		result.Add(All.FindById(lastParam.NodeId) + All.GetByAncs(All.FindById(exp.NodeId)));
		
	}
}