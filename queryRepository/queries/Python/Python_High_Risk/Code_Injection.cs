CxList methods = Find_Methods();

CxList dynamicMethodInvoke = methods.FindByShortNames(new List<string>{"eval", "execfile", "exec"});

CxList inputs = Find_Interactive_Inputs();
CxList systemJs = All.FindByShortName("system_js");
CxList systemJsMembers = systemJs.GetMembersOfTarget();
CxList systemJsMemberAccess = systemJsMembers.FindByType(typeof(MemberAccess)).InfluencedBy(inputs);

foreach(CxList memberAccess in systemJsMemberAccess.GetCxListByPath()){
	CxList endNode = memberAccess.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
	CxList eachSystemJsMemberAccess = systemJsMembers.FindByShortName(endNode, false);
	CxList eachSystemInvoke = eachSystemJsMemberAccess.FindByType(typeof(MethodInvokeExpr));
	CxList concatenatePaths = memberAccess.ConcatenateAllPaths(eachSystemInvoke);
	result.Add(concatenatePaths);
}

result.Add(inputs.DataInfluencingOn(dynamicMethodInvoke));