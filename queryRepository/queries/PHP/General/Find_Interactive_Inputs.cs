CxList variables = All.FindByType(typeof(UnknownReference));

CxList methods = Find_Methods();
CxList strings = Find_Strings();

// Find HTTP variables
CxList http = All.FindByShortName(@"HTTP_*");
CxList httpRefs = variables * http;
result = 
	httpRefs.FindByShortNames(new List<string>(new string[] {
	"HTTP_GET_VARS", "HTTP_POST_VARS", "HTTP_ROW_GET_DATA", "HTTP_ROW_POST_DATA", "HTTP_COOKIE_VARS","HTTP_POST_FILES"})) +
	
	variables.FindByShortNames(new List<string>(new string[] {
	"_POST", "_GET", "_REQUEST", "_FILES", "_COOKIE"})) -

	Find_Left_Side_Sanitize();

// Find HTTP strings that are safe for 
CxList unsafeEnvParams = strings * http;
CxList server = strings.FindByShortName(@"SERVER_*");
List<String> strSafeServerParamsStrings = new List<String> {
		// Server controlled
		"GATEWAY_INTERFACE","DOCUMENT_ROOT", "SERVER_ADDR", "SERVER_SOFTWARE", "SERVER_ADMIN", "SERVER_SIGNATURE",
		// Partly server controlled
		"HTTPS","REQUEST_TIME", "REMOTE_ADDR", "REMOTE_HOST", "REMOTE_PORT", "SCRIPT_FILENAME",
		"SCRIPT_NAME","SERVER_PROTOCOL","SERVER_NAME","SERVER_PORT",
		//The fields below can't cause to vulnerability
		"REMOTE_ADDR", "REQUEST_TIME_FLOAT", "REDIRECT_REMOTE_USER"};

CxList safeServerParams = strings.FindByShortNames(strSafeServerParamsStrings) +
	//The fields below can't cause to vulnerability
	unsafeEnvParams.FindByShortNames(new List<string> 
	{"HTTP_CLIENT_IP", "HTTP_X_FORWARDED_FOR", "HTTP_X_FORWARDED_SERVER", "HTTP_X_FORWARDED_HOST"});

//Select all unknown references of "$_SERVER" 
CxList Server = variables.FindByShortName("_Server", false);
//Server indexes are accessed as followed : "$_SERVER['PARAMETER']", therefore, the fathers of the parameters are of type IndexerRef. 
CxList IRefs = safeServerParams.GetFathers().FindByType(typeof(IndexerRef));
CxList SafeServer = All.NewCxList();
//For each IndexerRef, get its target and use the id to isolate the safe reference of $_SERVER.
foreach(CxList IRef in IRefs){
	IndexerRef IRefDom = IRef.data.GetByIndex(0) as IndexerRef;
	SafeServer.Add(Server.FindById(IRefDom.Target.NodeId));
}
//Remove safe references of $_SERVER from the results.
result.Add(Server - SafeServer);

// Add handle ENV here

unsafeEnvParams -= unsafeEnvParams.FindByShortName(@"HTTP_CLIENT_IP");
unsafeEnvParams -= unsafeEnvParams.FindByShortName(@"HTTP_X_FORWARDED_FOR");
unsafeEnvParams -= unsafeEnvParams.FindByShortName(@"HTTP_X_FORWARDED_SERVER");
unsafeEnvParams.Add(strings.FindByShortName(@"REQUEST_URI"));
unsafeEnvParams.Add(strings.FindByShortName(@"QUERY_STRING"));

result.Add(methods.FindByShortName("getenv").FindByParameters(unsafeEnvParams));
result.Add(unsafeEnvParams.GetAncOfType(typeof(IndexerRef)).FindByShortName("*_ENV"));
CxList argv = All.FindByShortName("argv").FindByType(typeof(IndexerRef));
result.Add(variables.FindAllReferences(argv));

// 'filter_input' method
CxList filter_inputs = methods.FindByShortName("filter_input");
filter_inputs = All.GetParameters(filter_inputs, 0);

result.Add(filter_inputs);
result.Add(Find_Zend_Interactive_Inputs());
result.Add(Find_Kohana_Interactive_Inputs());
result.Add(Find_Cake_Interactive_Inputs());
result.Add(Find_Symfony_Interactive_Inputs());

//memcache: add get methods influenced by interactive inputs
result.Add(Find_memcache_Inputs(result));