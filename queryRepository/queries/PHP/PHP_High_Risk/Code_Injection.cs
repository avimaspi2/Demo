CxList methods = Find_Methods();
CxList dynamic_method_invoke = methods.FindByShortNames(new List<string>()
	{ "$_Function", "call_user_func*", "forward_static_call*", "register_shutdown_function", "register_tick_function" });

CxList dynamic_method_creation = methods.FindByShortNames(new List<string>() { "create_function", "eval", "xpath_eval" });
CxList dynamic_variable_access = methods.FindByShortName("$_Variable");

CxList influencedBydbRefFunc = All.InfluencedBy(All.FindByShortName("*Reflection*"));
CxList reflectionInvoke = All.FindByShortName("invoke*").GetTargetOfMembers() * influencedBydbRefFunc;
dynamic_method_invoke.Add(reflectionInvoke);

CxList firstParam = dynamic_method_invoke;
firstParam = All.GetParameters(firstParam, 0);

CxList inputs = Find_Interactive_Inputs();
CxList sanitize = Find_Code_Injection_Sanitize();

result = inputs.InfluencingOnAndNotSanitized(dynamic_method_creation + dynamic_variable_access, sanitize);

CxList arrays = methods.FindByShortName("array");

CxList arraysSecondParam = All.GetParameters(arrays, 1);
CxList arraysThirdParam = All.GetParameters(arrays, 2);
CxList relevantArrays = arraysSecondParam.GetAncOfType(typeof(MethodInvokeExpr)) - arraysThirdParam.GetAncOfType(typeof(MethodInvokeExpr));
CxList arraysFirstParam = All.GetParameters(relevantArrays, 0);

result.Add(inputs.InfluencingOnAndNotSanitized(firstParam, sanitize + arraysFirstParam));