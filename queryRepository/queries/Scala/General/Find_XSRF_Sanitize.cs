/*Insert sanitize methods from frameworks and csrf projects*/

CxList methods = Find_Methods();
CxList csrf_related_methods = methods.FindByShortNames(new List<string> {
		//OWASP CSRFGuard Project
		"isValidRequest",
		// GWT
		"getNewXsrfToken",
		"setRpcToken", 
		//ESAPI
		"verifyCSRFToken",
		//Struts
		"isTokenValid"}); 

CxList var_names = Find_UnknownReference() ;
CxList possible_token_names = 
	var_names.FindByShortNames(new List<string> {
		"*token*",
		"*csrf*",
		"*xsrf*",
		"nonce",
		"*captcha*"},false);

/* Akka */
CxList mFathersAkka = methods.FindByName("randomTokenCsrfProtection");
CxList funcAkka = methods.GetByAncs(mFathersAkka);
CxList aux = funcAkka.FindByShortNames(new List<string>{"setSession","requiredSession","invalidateSession"});

result = (possible_token_names + csrf_related_methods).GetAncOfType(typeof(MethodDecl));
result.Add(aux);