CxList methods = Find_Methods();

CxList dynamicMethodInvoke = methods.FindByShortNames(new List<string>{"eval", "execfile", "exec"});

CxList inputs = Find_DB_Out();
inputs.Add(Find_Read());

CxList systemJs = Find_MemberAccesses().FindByShortName("system_js");
CxList systemJsMembers = systemJs.GetMembersOfTarget();
CxList systemJsMemberAccess = systemJsMembers.FindByType(typeof(MemberAccess)).InfluencedBy(inputs);

foreach(CxList memberAccess in systemJsMemberAccess.GetCxListByPath()){
	CxList endNode = memberAccess.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
	CxList eachSystemJsMemberAccess = systemJsMembers.FindByShortName(endNode, false);
	CxList eachSystemInvoke = eachSystemJsMemberAccess * methods;
	CxList concatenatePaths = memberAccess.ConcatenateAllPaths(eachSystemInvoke);
	result.Add(concatenatePaths);
}

result.Add(inputs.DataInfluencingOn(dynamicMethodInvoke));