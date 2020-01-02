if ((All.FindByFileName(@"*.cshtml").Count > 0 || All.FindByFileName(@"*.aspx").Count > 0) &&
	(All.FindByFileName(@"*controllers*").Count > 0 || All.FindByFileName(@"*views*").Count > 0))
{
	//Find errors that will be outputs.
	CxList errorOutput = All.FindByMemberAccess("ModelState.AddModelError");
	errorOutput = All.GetParameters(errorOutput, 1);
	
	CxList methodInvokes = All.FindByType(typeof(MethodInvokeExpr));
	CxList returnStmts = All.FindByType(typeof(ReturnStmt));
	returnStmts = returnStmts.GetByAncs(Find_ASP_MVC_Controllers());
	
	//Find return statements in controllers that are outputs.
	List<string> returnTypes = new List<string> {"Content", "JavaScript", "Json"};
	CxList controllerOutput = methodInvokes.FindByShortNames(returnTypes);
	controllerOutput = controllerOutput.FindByFathers(returnStmts);
	
	// MVC Controllers can also return a string (implicitely wrapped into an ActionResult)
	CxList controllersReturningString = Find_ASP_MVC_Controllers().FindByMethodReturnType("string");
	CxList stringReturnedByController = All.FindByFathers(returnStmts.GetByAncs(controllersReturningString));
	
	result.Add(errorOutput);
	result.Add(controllerOutput);
	result.Add(stringReturnedByController);
}