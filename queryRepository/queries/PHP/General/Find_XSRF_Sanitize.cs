CxList methods = Find_Methods();

CxList csrf_related_methods = 
	//OWASP CSRFGuard Project
	methods.FindByShortNames(new List<string>
	{"csrfguard*",
	//Symfony Framework
	"addCSRFProtection","getCSRFToken","enableCSRFProtection",
	//Zend Framework
	"setCSRFValidator", "isValid","Zend_Form_Element_Hash",
	//Joomla
	"checkToken",
	//Kohana
	"check",
	//CodeIgniter
	"csrf_verify",
	//Yii
	"validateCsrfToken",
	//Wordpress
	"wp_verify_nonce",
	//ESAPI
	"verifyCSRFToken"});

/*
CxList csrf_related_methods = 
	//OWASP CSRFGuard Project
	methods.FindByShortName("csrfguard*") + 
	//Symfony Framework
	methods.FindByShortName("addCSRFProtection") + 
	methods.FindByShortName("getCSRFToken") + 
	methods.FindByShortName("enableCSRFProtection") + 
	//Zend Framework
	methods.FindByShortName("setCSRFValidator") + 
	methods.FindByShortName("isValid") + 
	methods.FindByShortName("Zend_Form_Element_Hash") +
	//Joomla
	methods.FindByShortName("checkToken") +
	//Kohana
	methods.FindByShortName("check") + 
	//CodeIgniter
	methods.FindByShortName("csrf_verify") +
	//Yii
	methods.FindByShortName("validateCsrfToken") +
	//Wordpress
	methods.FindByShortName("wp_verify_nonce") +
	//ESAPI
	methods.FindByShortName("verifyCSRFToken");
*/	
CxList var_names = All.FindByType(typeof(UnknownReference));
CxList possible_token_names = 
	var_names.FindByShortName("*token*") +
	var_names.FindByShortName("*csrf*") +
	var_names.FindByShortName("*xsrf*") +
	var_names.FindByShortName("nonce") +
	var_names.FindByShortName("*captcha*");

CxList res = possible_token_names + csrf_related_methods;

result = res.GetAncOfType(typeof(MethodDecl)) + 
	res.GetAncOfType(typeof(IfStmt));
result.Add(Find_Cake_XSRF_Sanitizer());