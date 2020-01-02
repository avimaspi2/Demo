/*Insert sanitize methods from frameworks and csrf projects*/

CxList methods = Find_Methods();
CxList csrfRelatedMethods = methods.FindByShortNames(new List<string> {
		//OWASP CSRFGuard Project
		// GWT
		"getNewXsrfToken",
		"setRpcToken",
		//ESAPI
		//methods.FindByShortName("addCSRFToken") + 
		//methods.FindByShortName("getCSRFToken") + 
		"verifyCSRFToken",
		//Struts
		//methods.FindByShortName("saveToken") + 
		//methods.FindByShortName("resetToken") + 
		"isTokenValid"}); 

CxList varNames = Find_UnknownReference() ;
CxList possibleTokenNames = 
	varNames.FindByShortNames(new List<string> {
		"*token*",
		"*csrf*",
		"*xsrf*",
		"nonce",
		"*captcha*"});

CxList tokenMethods = All.NewCxList();
tokenMethods.Add(possibleTokenNames);
tokenMethods.Add(csrfRelatedMethods);
	
result = tokenMethods.GetAncOfType(typeof(MethodDecl));