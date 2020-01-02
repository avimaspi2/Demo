if (Find_Extend_Zend().Count > 0)
{
	CxList trc = All.FindByType(typeof(TypeRefCollection));
	CxList extend = All.FindByType(typeof(TypeRef)).GetByAncs(trc);
	CxList extendActionController = extend.FindByShortName("*Controller*");
	CxList controllerClass = extendActionController.GetAncOfType(typeof(ClassDecl));
	CxList controllerRef = All.FindAllReferences(controllerClass);
	CxList controllerRequest = controllerRef.GetMembersOfTarget();
	result.Add(controllerRequest.FindByShortName("*getParam*") + controllerRequest.FindByShortName("*getAllParam*"));
	
	//remove _getParam('error_handler')
	result = result - All.GetParameters(result).FindByShortName("\"error_handler\"").GetAncOfType(typeof(MethodInvokeExpr));
	
	CxList methods = Find_Methods();
	CxList memberAccess = All.FindByType(typeof(MemberAccess));
	CxList methodsMemberAccess = methods + memberAccess;

	CxList zendControllerRequest = methodsMemberAccess.FindByShortNames(new List<string> {
		"__get",
		"get",		
		"post",
		"getPost",
		"getQuery",
		"getCookie",		
		"getEnv",	
		"getServer",
		"getParam*",		
		"_getParam*",		
		"getAllParam*",		
		"_getAllParam*",		
		"getActionKey",		
		"getUserParam*",		
		"getRawBody",	
		"getHeader*"}); 		
/*		
		methodsMemberAccess.FindByShortName("__get") +
		methodsMemberAccess.FindByShortName("get") +
		methodsMemberAccess.FindByShortName("post") +
		methodsMemberAccess.FindByShortName("getPost") +
		methodsMemberAccess.FindByShortName("getQuery") +
		methodsMemberAccess.FindByShortName("getCookie") +
		methodsMemberAccess.FindByShortName("getEnv") +
		methodsMemberAccess.FindByShortName("getServer") +
		methodsMemberAccess.FindByShortName("getParam*") +
		methodsMemberAccess.FindByShortName("_getParam*") +
		methodsMemberAccess.FindByShortName("getAllParam*") +
		methodsMemberAccess.FindByShortName("_getAllParam*") +
		methodsMemberAccess.FindByShortName("getActionKey") +
		methodsMemberAccess.FindByShortName("getUserParam*") +
		methodsMemberAccess.FindByShortName("getRawBody") +
		methodsMemberAccess.FindByShortName("getHeader*") + 
		memberAccess.FindByShortName("params") +
		memberAccess.FindByShortName("rawBody");
*/	
	zendControllerRequest.Add(memberAccess.FindByShortNames(new List<string> {"params","rawBody"}));
	

	CxList unknownRef = All.FindByType(typeof(UnknownReference));
	CxList request = 
		(unknownRef + memberAccess).FindByShortName("*request*") + 
		methods.FindByShortName("getRequest", false);
	result.Add((zendControllerRequest.GetTargetOfMembers() * request).GetMembersOfTarget());

	
	//handle forms
	CxList myClass = All.FindByType(typeof(ClassDecl));
	CxList oce = All.FindByType(typeof(ObjectCreateExpr));
	CxList zendFilteredInputCalls = 
		methods.FindByShortName("getValue*") +
		methods.FindByShortName("getUnfilteredValue*") +
		methods.FindByShortName("getValidValue*");
	CxList inheritFromForm = myClass.InheritsFrom("Zend_Form") + myClass.InheritsFrom("*Form*");
	CxList form = zendFilteredInputCalls.GetTargetOfMembers();
	CxList allForms = unknownRef.FindByShortName(form).FindAllReferences(form);
	CxList formInf = allForms.DataInfluencingOn(form);
	CxList formOce = oce.GetByAncs(formInf.GetFathers());
	formOce = formOce.FindByShortName(inheritFromForm);
	CxList ae = formOce.GetFathers();
	formInf = formInf.GetByAncs(ae);
	result.Add((form.DataInfluencedBy(formInf)).GetMembersOfTarget());
	
	/*
	When user sends a request for a non-existing page, the related Zend controller which is called does not find 
	the related action method and therefore invokes his "__call" method (which is a PHP magic function).
	The first parameter of "__call" is the name of the non-existing action method.
	This parameter is regarded as interactive input since it can be recieved from the user request.
	*/
	CxList magicMethod = All.FindByType(typeof(MethodDecl)).FindByShortName("__call");
	magicMethod = magicMethod.GetByAncs(controllerRef);
	CxList magicMethodNameParam = All.GetParameters(magicMethod, 0);
	result.Add(All.GetByAncs(magicMethodNameParam));
	        
}