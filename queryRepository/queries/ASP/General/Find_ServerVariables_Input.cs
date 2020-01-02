/*
Find ServerVariables parameters that are inputs
*/
CxList allMethods = All.FindByType(typeof (MethodInvokeExpr));
CxList allParams = All.FindByType(typeof (Param));
CxList allStrings = Find_Strings() - Find_Empty_Strings();

CxList ServerVariables = allMethods.FindByShortName("ServerVariables", false);
CxList allServerVariablesParams = allParams.GetParameters(ServerVariables);

//get ServerVaraibles collection:
CxList svColl = (All - ServerVariables).FindByMemberAccess("Request.ServerVariables", false);

CxList allParamsAsStrings = allStrings.GetByAncs(allServerVariablesParams);

result =
	allParamsAsStrings.FindByShortName("\"ALL_HTTP\"", false) +
	allParamsAsStrings.FindByShortName("\"AUTH_PASSWORD\"", false) +
	allParamsAsStrings.FindByShortName("\"AUTH_USER\"", false) +
	allParamsAsStrings.FindByShortName("\"CERT_ISSUER\"", false) +
	allParamsAsStrings.FindByShortName("\"CERT_SERIALNUMBER\"", false) +
	allParamsAsStrings.FindByShortName("\"CERT_SUBJECT\"", false) +
	allParamsAsStrings.FindByShortName("\"CONTENT_TYPE\"", false) +
	allParamsAsStrings.FindByName("\"HTTP_*", false) +
	allParamsAsStrings.FindByShortName("\"PATH_INFO\"", false) +
	allParamsAsStrings.FindByShortName("\"QUERY_STRING\"", false) +
	allParamsAsStrings.FindByShortName("\"REMOTE_HOST\"", false) +
	allParamsAsStrings.FindByShortName("\"REMOTE_USER\"", false) +
	allParamsAsStrings.FindByShortName("\"URL\"", false);

result.Add(svColl);