CxList testingMethodDecls = Find_MethodDecls_Testing();

CxList allMethodDecls = Find_MethodDecls();
allMethodDecls -= testingMethodDecls;

CxList allMethods = Find_Methods();
allMethods -= allMethods.GetByAncs(testingMethodDecls);


CxList errorHandlers = Get_Error_Handlers();
errorHandlers.Add(Find_Conditions());

CxList recoverMethods = allMethods.FindByShortName("recover");
CxList panicMethods = allMethods.FindByShortName("panic");
CxList deferMethods = All.FindByCustomAttribute("defer").GetAncOfType(typeof(MethodInvokeExpr));

CxList recoversInDefer = recoverMethods.GetByAncs(allMethodDecls.FindDefinition(deferMethods));
CxList loggedRecovers = errorHandlers.DataInfluencedBy(recoversInDefer).GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);

CxList loggedRecoversCall = loggedRecovers.GetFathers().GetAncOfType(typeof(MethodInvokeExpr));
loggedRecoversCall.Add(All.FindAllReferences(allMethods.GetMethod(loggedRecovers)));


CxList alreadyProcessed = All.NewCxList();
CxList currentList = allMethods.GetMethod(loggedRecoversCall * deferMethods);
CxList nextList = All.NewCxList();

CxList panicMethodsHandled = All.NewCxList();
CxList panicInvoque = panicMethods.GetAncOfType(typeof(MethodDecl));


CxList methodsWithPanicCalls = allMethods.FindAllReferences(panicInvoque);


while (currentList.Count > 0)
{	
	foreach (CxList currentMethod in currentList)
	{
		MethodDecl methodStmt = currentMethod.TryGetCSharpGraph<MethodDecl>();
		if(alreadyProcessed.ContainsKey(methodStmt.NodeId))
			continue;
		
		alreadyProcessed.Add(currentMethod);
		
		//Get all method invoques on current method
		CxList methodInvoques = allMethods.GetByAncs(currentMethod);
		
		CxList inters = methodInvoques * methodsWithPanicCalls;
		if(inters.Count > 0)//a panic is handled
		{
			panicMethodsHandled.Add(inters);
		}
		else // continue search
		{
			nextList.Add(allMethodDecls.FindDefinition(methodInvoques));
		}
	}
	currentList = nextList.Clone();
	nextList.Clear();
}

CxList handledPanicMethods = panicMethods.GetByAncs(panicInvoque.FindDefinition(panicMethodsHandled));
result = panicMethods - handledPanicMethods;