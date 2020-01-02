//This general query adds selector flow when using the method self.performSelector:withObject: 
//This is the way Objc has to obtain dynamically methods in the same object

CxList methods = Find_Methods();
CxList parameters = Find_Param();
//Find the NSSelectorFromString functions
List<string> methodsNames = new List<string>{"NSSelectorFromString", "NSSelectorFromString:"};
CxList selectorMethods = methods.FindByShortNames(methodsNames);

//Find the method reference invoke 
foreach (CxList selectorMethod in selectorMethods){
	CxList methodNameInSelector = parameters.GetParameters(selectorMethod);
	// extract name
	string methodName = methodNameInSelector.GetName();
	methodName = methodName.Substring(2, methodName.Length - 2);

	//Find the method parameters
	CxList selector = selectorMethod.GetFathers();
	if(selector.FindByType(typeof(AssignExpr)).Count>0)
		selector = All.GetByAncs(selector).FindByAssignmentSide(CxList.AssignmentSide.Left);
	
	CxList methodInvokes = methods.FindByParameters(All.FindAllReferences(selector));
	methodInvokes = methodInvokes.FindByShortName("perform*");	
	
	//find method declaration and the parameters 
	CxList methodDecl = All.FindByShortName(methodName).FindByType(typeof(MethodDecl));

	//Set the custom flow between the reference parameters and the method declaration parameters 
	for(int i = 0;i < 2;i++){
		CxList methodDeclParam = All.GetParameters(methodDecl, i);
		if(methodDeclParam.Count == 0)
			break;
		CxList methodInvokesParam = All.GetParameters(methodInvokes, i + 1);
		CustomFlows.AddFlow(methodInvokesParam, methodDeclParam);
	}
}