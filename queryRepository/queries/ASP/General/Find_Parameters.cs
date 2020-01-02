CxList parameters = Find_Member_With_Target("ADODB.Command", "Parameters*");

result = 
	parameters + 
	parameters.GetMembersOfTarget() + 
	parameters.GetAncOfType(typeof(MethodInvokeExpr));