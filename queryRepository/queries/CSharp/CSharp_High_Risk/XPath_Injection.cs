CxList XPath = Find_XPath_Output();
CxList inputs = Find_Interactive_Inputs();
CxList sanitized = Find_Sanitize();

CxList replace = All.FindByName("*.Replace*");
replace = replace.FindByType(typeof(MemberAccess)) + replace.FindByType(typeof(MethodInvokeExpr));

//sanitized should not bring any Replace since camouflages vulnerabilities
replace.Add(XPath);
sanitized -= replace;

result = XPath.InfluencedByAndNotSanitized(inputs, sanitized).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);