//We get all sensitive data. 
CxList personal_info = Find_Personal_Info();

//We remove strings, since they might contain: "Enter password".
//A potential problem is that it might also contain: "password is...", but then it's hardcoded, 
//and not really sensitive information.
personal_info -= Find_Strings();
personal_info -= Find_Integers();

CxList removeRegexPattern = personal_info.FindByShortNames(new List<string> {"*regex*", "*pattern*"},false);

personal_info -= removeRegexPattern;

//Remove declarators that are null or have an empty string assigned to it from personal_info
CxList nullOrEmpty = Find_Null_String_Name();
nullOrEmpty.Add(Find_Empty_Strings());
CxList assignedNull = nullOrEmpty.GetFathers() * personal_info;
assignedNull.Add(personal_info.FindByFathers(nullOrEmpty.GetFathers()));
personal_info -= assignedNull;

//We deal with 2 types of risky outputs - HttpServletResponse and Socket
CxList outputs = Find_Outputs();

//1. HttpServletResponse must be checked by HttpServletRequest.isSecure() if it is secure

CxList response = All.FindByTypes(new String[]{"HttpServletResponse","ServletResponse"});
CxList webOutputsParams = outputs.FindByType(typeof(Param));   
CxList webOutputsMethods = Find_Methods().FindByParameters(webOutputsParams);
CxList outputsResponse = All.NewCxList();
outputsResponse.Add(outputs);
outputsResponse.Add(webOutputsMethods);
outputsResponse = outputsResponse.DataInfluencedBy(response);

CxList conditions = Find_Conditions();
CxList isSecure = conditions.FindByMemberAccess("HttpServletRequest.isSecure");
isSecure.Add(conditions.FindByMemberAccess("HTTPUtilties.isSecureChannel"));
CxList secureIf = isSecure.GetFathers();
outputsResponse -= outputsResponse.GetByAncs(secureIf);

//Parameters of HttpServletResponse objects
CxList allParams = All.GetParameters(webOutputsMethods);  
CxList methods = outputsResponse.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly); 
CxList outputsResponseParams = allParams.GetParameters(methods);  

//Relevant methods
CxList outputRelevantMethods = methods - webOutputsMethods;
CxList writeMethods = outputRelevantMethods.FindByShortNames(new List<string> {"write*", "print*", "append*"});
outputRelevantMethods -= writeMethods;
outputRelevantMethods.Add(All.GetParameters(writeMethods)); 

//2. Socket is always insecure (should be SSLSocket to be secured)
CxList socket = All.FindByType("Socket");
socket.Add(All.FindByType("ServerSocket"));

//Secure 
CxList wrapSSL = All.FindByMemberAccess("SSLEngine.wrap");
CxList wrap_param = All.FindAllReferences(All.GetParameters(wrapSSL, 1)); //Get output from wrap(passed by reference)

//Outputs that use secure parameters
CxList sanitized_outputs = wrap_param.DataInfluencingOn(outputs).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

//Sockets that influence outputs that dont have secure parameters
CxList outputsNotSanitized = outputs - sanitized_outputs;
CxList outputsSocket = socket.DataInfluencingOn(outputsNotSanitized).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

outputs = outputRelevantMethods;
outputs.Add(outputsSocket); 
outputs.Add(outputsResponseParams);

//Anything that passes through the DB now has info from the DB and not the sensitive data
CxList sanitize = Find_DB_In();
sanitize.Add(Find_Dead_Code_Contents());

//Add encryption as sanitizer
sanitize.Add(Find_Encrypt());

//Relevant parameters from HttpServletResponse methods 
CxList jspResponseParams = outputsResponseParams * Find_Jsp_Code();
CxList relevantParameters = (outputs - jspResponseParams) * personal_info;
CxList relevantJspParameters = jspResponseParams * personal_info;
relevantJspParameters = relevantJspParameters.ReduceFlowByPragma();
relevantParameters.Add(relevantJspParameters);

result = outputs.InfluencedByAndNotSanitized(personal_info, sanitize);

//Remove nodes that are part of outputs, but of type Param
CxList realOutputs = result.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
CxList realOutputsAsParam = realOutputs.GetAncOfType(typeof(Param));
result.Add(relevantParameters - sanitize - realOutputsAsParam);

result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);