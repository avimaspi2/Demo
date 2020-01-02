// The purpose of the query to find Stored XSS vulnerabilities
// when cURL object is used
// ------------------------------------------------------------

CxList outputs = Find_Interactive_Outputs();
CxList sanitize = Find_XSS_Sanitize();

CxList curl_setoptMethods = Find_Methods().FindByName("*curl_setopt*");
CxList curl_execMethods = Find_Methods().FindByName("*curl_exec*");


// >> The block below handle case of vulnerable use of exec() operator with CURLOPT_URL operand
CxList firstParam = All.GetParameters(curl_setoptMethods, 0);
CxList secondParam = All.GetParameters(curl_setoptMethods, 1);
CxList thirdParam = All.GetParameters(curl_setoptMethods, 2);

CxList secondParamsCURLOPT_URL = secondParam.FindByShortName("CURLOPT_URL", false);
CxList secondParamsCURLOPT_RETURNTRANSFER = secondParam.FindByShortName("CURLOPT_RETURNTRANSFER", false);
CxList thirdParamsTrue = thirdParam.FindByShortName("true", false) + secondParam.FindByShortName("1") ;


CxList firstParamAsInput = All.GetParameters(secondParamsCURLOPT_URL.GetAncOfType(typeof(MethodInvokeExpr)), 0);
CxList firstParamAsIndirectModeInput = All.GetParameters(secondParamsCURLOPT_RETURNTRANSFER.GetAncOfType(typeof(MethodInvokeExpr)), 0) *
	                                   All.GetParameters(thirdParamsTrue.GetAncOfType(typeof(MethodInvokeExpr)), 0);

CxList firstParamAsPipeModeInput = All.FindAllReferences(firstParamAsInput) - All.FindAllReferences(firstParamAsIndirectModeInput);
// << end block

CxList curl_execMethodsIndirectMode = curl_execMethods.FindByAssignmentSide(CxList.AssignmentSide.Right);
CxList curl_execMethodsPipeMode = curl_execMethods - curl_execMethodsIndirectMode;

// >> The block below handle case of vulnerable use init(url)  and exec
CxList curl_initMethods = Find_Methods().FindByName("*curl_init*");
CxList curl_initParams = All.GetParameters(curl_initMethods, 0);
CxList relevantcurl_initMethods = curl_initParams.GetAncOfType(typeof(MethodInvokeExpr));
CxList vulnerableInit = curl_execMethodsPipeMode.InfluencedByAndNotSanitized(relevantcurl_initMethods, sanitize + All.FindAllReferences(firstParamAsIndirectModeInput));
// << end block

// >> The block below handle case of vulnerable use of get_info with CURLINFO_HTTP_CODE operand
// For example:
// 			$status = curl_getinfo($ch, CURLINFO_HTTP_CODE); 
// 			echo $ status; //XSS 

CxList curl_getinfoMethods = Find_Methods().FindByName("*curl_getinfo*");
CxList curl_getinfoParams = All.GetParameters(curl_getinfoMethods, 1).FindByShortName("CURLINFO_HTTP_CODE", false);
CxList relevant_curl_getinfoParamsMethods = curl_getinfoParams.GetAncOfType(typeof(MethodInvokeExpr));
CxList vulnerableGetInfo = outputs.InfluencedByAndNotSanitized(relevant_curl_getinfoParamsMethods, sanitize);
// << end block

CxList curl_multi_getcontentMethods = Find_Methods().FindByName("*curl_multi_getcontent*");
CxList vulnerableGetConetent = outputs.InfluencedByAndNotSanitized(curl_multi_getcontentMethods, sanitize);
	
CxList vulnerableCURLOPT_URL = All.FindAllReferences(firstParamAsPipeModeInput).GetParameters(curl_execMethodsPipeMode);
CxList vulnerableCURLOPT_RETURNTRANSFER = outputs.InfluencedByAndNotSanitized(All.FindAllReferences(firstParamAsIndirectModeInput).GetParameters(curl_execMethodsIndirectMode), sanitize);

result = vulnerableGetConetent + 
	     vulnerableInit + 
	     vulnerableCURLOPT_URL + 
	     vulnerableCURLOPT_RETURNTRANSFER +
		 vulnerableGetInfo;