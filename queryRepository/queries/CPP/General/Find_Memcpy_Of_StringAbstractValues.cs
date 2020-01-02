CxList memcpyMethods = Find_Methods().FindByShortName("memcpy");
CxList memcpyMethodsWithStringAbstractValue = (All.GetParameters(memcpyMethods, 1) - Find_Parameters())
	.FindByAbstractValue(abstractValue => abstractValue is StringAbstractValue)
	.GetAncOfType(typeof(MethodInvokeExpr));

result = memcpyMethodsWithStringAbstractValue;