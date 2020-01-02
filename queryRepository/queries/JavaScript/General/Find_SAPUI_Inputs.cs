if(cxScan.IsFrameworkActive("SAPUI")){
		
	CxList unknownRef = Find_UnknownReference();
	CxList paramDecl = Find_ParamDecl();
	CxList methods = Find_Methods();
	CxList lambdaExpr = Find_LambdaExpr();
	CxList fieldDecls = Find_FieldDecls();
		
	// get the controllers => sap/ui/core/mvc/Controllers
	CxList controllers = Find_SAPUI_Extending_Objects("sap/ui/core/mvc/Controller");

	// Collect getTarget(s)
	List<string> getTargetStrings = new List<string>{
			"getTarget",
			"getTargets"
			};
	
	// Collect getParameters
	List<string> getMParameters = new List<string>{
			"getParameter",
			"getParameters",
			"mParameters"
			};
		
	// Find Methods
	CxList controllerMethods = methods.GetByAncs(controllers);
	CxList getRouterMethod = controllerMethods.FindByShortName("getRouter");
	CxList getTargetMethod = controllerMethods.FindByShortNames(getTargetStrings);
	CxList attachDisplayMethod = controllerMethods.FindByShortName("attachDisplay"); 
	CxList ownerComponentMethod = controllerMethods.FindByShortName("getOwnerComponent");
		
	// get the controllers '_onRouteMatched'
	CxList onRouteMatchedFieldDecls = fieldDecls.GetByAncs(controllers).FindByShortName("_onRouteMatched");
	CxList controllersOnRouteMatchedLambda = lambdaExpr.GetByAncs(onRouteMatchedFieldDecls);

	// get oEvent references
	CxList oEventParams = paramDecl.GetParameters(controllersOnRouteMatchedLambda, 0);
	CxList oEventRefs = unknownRef.FindAllReferences(oEventParams);
	
	// Sequential way =>  this.getOwnerComponent().getRouter().getTargets().attachDisplay(function(evt){
	CxList getRouter = ownerComponentMethod.GetMembersOfTarget().FindByShortName(getRouterMethod);
	CxList getTarg = getRouter.GetMembersOfTarget().FindByShortName(getTargetMethod);
	CxList attachDisplay = getTarg.GetMembersOfTarget().FindByShortName(attachDisplayMethod); 
	
	// Splitted way
	// Find the router collecting from the OwnerComponent
	CxList router = getRouter.GetAssignee();
	CxList routerRefences = unknownRef.FindAllReferences(router); 
		
	// Find the getTarget over this router
	CxList getTarget = routerRefences.GetMembersOfTarget().FindByShortName(getTargetMethod);
	CxList target = getTarget.GetAssignee();
	CxList targetReferences = unknownRef.FindAllReferences(target); 
	
	// Find the handler for the attachDisplay method on the target
	attachDisplay.Add(targetReferences.GetMembersOfTarget().FindByShortName(attachDisplayMethod)); 
	CxList eventFunction = lambdaExpr.GetParameters(attachDisplay, 0);
	eventFunction.Add(lambdaExpr.GetParameters(attachDisplay, 1));
	
	// Get attachDisplay Event Function Param inputs 
	CxList eventFunctionParam = paramDecl.GetParameters(eventFunction.GetAncOfType(typeof(LambdaExpr)), 0);
	CxList eventFunctionParamRef = unknownRef.FindAllReferences(eventFunctionParam);
	result = eventFunctionParamRef.GetMembersOfTarget().FindByShortNames(getMParameters);
	result.Add(oEventRefs.GetMembersOfTarget().FindByShortNames(getMParameters));
}