CxList methods = Find_Methods();
CxList dynamic_method_invoke = methods.FindByShortNames(new List<String>()
	{ "$_Function", "call_user_func*", "forward_static_call*", "create_function", 
		"register_shutdown_function", "register_tick_function", "eval" });

CxList dynamic_variable_access = methods.FindByShortName("$_Variable");

CxList reflectionInvoke = All.FindByShortName("invoke*").GetTargetOfMembers().DataInfluencedBy(All.FindByShortName("*Reflection*"));

dynamic_method_invoke.Add(reflectionInvoke);

CxList db = Find_DB_Out() + Find_Read();

CxList sanitize = Find_Code_Injection_Sanitize();
result = db.InfluencingOnAndNotSanitized(dynamic_method_invoke + dynamic_variable_access, sanitize);
               
CxList firstParam = dynamic_method_invoke;
firstParam = All.GetParameters(firstParam, 0);

CxList arrays = methods.FindByShortName("array");

CxList arraysSecondParam = All.GetParameters(arrays, 1);
CxList arraysThirdParam = All.GetParameters(arrays, 2);
CxList relevantArrays = arraysSecondParam.GetAncOfType(typeof(MethodInvokeExpr)) - arraysThirdParam.GetAncOfType(typeof(MethodInvokeExpr));
CxList arraysFirstParam = All.GetParameters(relevantArrays, 0);

result.Add(db.InfluencingOnAndNotSanitized(firstParam, sanitize + arraysFirstParam));
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);