CxList action = 
	All.FindByShortNames(new List<string> {
		"onSubmit",
		"onBind", 
		"validatePage",
		"processFinish",
		"processCancel",
		"doSubmitAction",
		"onFormChange", 
		"processFormSubmission",
		"referenceData"});

CxList controller = Find_Controllers();

action = action.GetByAncs(controller);	
CxList submitParam = All.GetParameters(action).FindByType("object");

CxList ThrowAwayController = All.InheritsFrom("ThrowAwayController");
CxList ThrowAwayControllerMembers = Find_Field_Decl().GetByAncs(ThrowAwayController);
CxList ThrowAwayControllerFields = All.FindAllReferences(ThrowAwayControllerMembers);

CxList springAnnotations = Find_Spring_Annotations_Inputs();

CxList customAttribute = Find_CustomAttribute();

// Consider members of references to @ModelAttribute as inputs
CxList springModelAttribute = customAttribute.FindByShortName("ModelAttribute").GetFathers() * springAnnotations;
springModelAttribute = All.FindAllReferences(springModelAttribute).GetMembersOfTarget();
springModelAttribute -= springModelAttribute.FindByShortNames(new List<string>{"set*", "put*"});
springAnnotations.Add(springModelAttribute);

CxList pathAttributes = customAttribute.FindByCustomAttribute("Path");
CxList pathClasses = pathAttributes.GetAncOfType(typeof(ClassDecl));
CxList pathMethods = pathAttributes.GetAncOfType(typeof(MethodDecl));
CxList methodDecl = Find_MethodDeclaration();
pathMethods.Add(methodDecl.GetByAncs(pathClasses));

CxList designatorsMethods = All.NewCxList();
designatorsMethods.Add(customAttribute.FindByCustomAttribute("GET"));
designatorsMethods.Add(customAttribute.FindByCustomAttribute("POST"));
designatorsMethods.Add(customAttribute.FindByCustomAttribute("PUT"));
designatorsMethods.Add(customAttribute.FindByCustomAttribute("DELETE"));
designatorsMethods.Add(customAttribute.FindByCustomAttribute("HEAD"));
designatorsMethods.Add(customAttribute.FindByCustomAttribute("OPTIONS"));
designatorsMethods.Add(customAttribute.FindByCustomAttribute("PATCH"));
designatorsMethods = designatorsMethods.GetAncOfType(typeof(MethodDecl));

CxList paramAnnotations = All.NewCxList();
paramAnnotations.Add(customAttribute.FindByCustomAttribute("MatrixParam"));
paramAnnotations.Add(customAttribute.FindByCustomAttribute("QueryParam"));
paramAnnotations.Add(customAttribute.FindByCustomAttribute("PathParam"));
paramAnnotations.Add(customAttribute.FindByCustomAttribute("CookieParam"));
paramAnnotations.Add(customAttribute.FindByCustomAttribute("HeaderParam"));
paramAnnotations.Add(customAttribute.FindByCustomAttribute("FormParam"));
paramAnnotations.Add(customAttribute.FindByCustomAttribute("Context"));
paramAnnotations = paramAnnotations.GetAncOfType(typeof(ParamDecl));

CxList pathDesignatorMethods = pathMethods * designatorsMethods;
CxList paramAnnotationParams = paramAnnotations.GetParameters(pathMethods);

springAnnotations.Add(All.GetParameters(pathDesignatorMethods));
springAnnotations.Add(paramAnnotationParams);
springAnnotations.Add(Get_Override_Method_Parameters(pathDesignatorMethods, paramAnnotationParams));

springAnnotations.Add(ThrowAwayControllerFields);
springAnnotations.Add(submitParam);

result.Add(springAnnotations);

// Parameters declarations from annoatation requests 
List <string> requestAnnotations = new List<string>{"RequestBody", "RequestParam"};
CxList requestParamAnnotations = customAttribute.FindByShortNames(requestAnnotations);
CxList requestParams = requestParamAnnotations.GetAncOfType(typeof(ParamDecl));
result.Add(requestParams);