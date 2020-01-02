CxList call_user = All.FindByShortNames(new List<String>()
	{ 
		"call_user_func", "call_user_func_array" }
	);

CxList call_user_fp = All.GetParameters(call_user,0);
CxList array_method = call_user_fp.FindByShortName("array");
array_method.Add(All.GetParameters(array_method));
call_user_fp -= array_method;

CxList call_user_fp_args =  All.GetParameters(call_user_fp.FindByType(typeof(MethodInvokeExpr)));

call_user = call_user_fp_args;

call_user.Add(All.GetParameters(call_user, 1));


result.Add(call_user);
result.Add(Find_General_Sanitize());
result.Add(Find_Encode());

result.Add(Find_HTML_Encode());