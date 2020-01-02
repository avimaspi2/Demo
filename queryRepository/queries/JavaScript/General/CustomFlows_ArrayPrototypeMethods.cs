List <string> methods = new List<string> {"forEach", "map", "filter", "reduce", "every", "some"}; 
CxList protoMethods = All.FindByType(typeof(MethodInvokeExpr)).FindByShortNames(methods);

foreach(CxList method in protoMethods){
	CxList targets = method.GetTargetOfMembers();
	CxList parameter = All.GetParameters(method,0).FindByType(typeof(LambdaExpr));
	
	if(parameter.Count > 0){
	
	CxList lambdaParam = All.GetParameters(parameter,0);
	CustomFlows.AddFlow(targets,lambdaParam);
	}
}