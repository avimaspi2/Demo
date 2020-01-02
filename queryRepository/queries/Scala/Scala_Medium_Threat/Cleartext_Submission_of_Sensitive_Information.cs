//We get all sensitive data
CxList personalInfo = Find_Personal_Info();

//We remove strings, since they might contain: "Enter password".
//A potential problem  is that it might also contain: "password is...", but then it's hardcoded, 
//and not really sensitive information.
personalInfo -= Find_Strings();
personalInfo -= Find_Integers();

//Remove declarators that are null or have an empty string assigned to it
CxList nullOrEmpty = Find_Null_String_Name();
nullOrEmpty.Add(Find_Empty_Strings());
CxList assignedNull = nullOrEmpty.GetFathers() * personalInfo;
assignedNull.Add(personalInfo.FindByFathers(nullOrEmpty.GetFathers()));
personalInfo -= assignedNull;

//We deal with 2 types of risky outputs - HttpServletResponse and Socket
CxList outputs = Find_Outputs();

//1. HttpServletResponse must be checked by HttpServletRequest.isSecure() if it is secure
CxList response = All.FindByType("HttpServletResponse");
CxList webOutputsParams = outputs.FindByType(typeof(Param));
CxList webOutputsMethods = Find_Methods().FindByParameters(webOutputsParams);
CxList outputsResponse = outputs;
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
CxList wrapParam = All.FindAllReferences(All.GetParameters(wrapSSL, 1)); //Get output from wrap(passed by reference)

//Outputs that use secure parameters
CxList sanitizedOutputs = wrapParam.DataInfluencingOn(outputs).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

//Sockets that influence outputs that dont have secure parameters
CxList outputsNotSanitized = outputs - sanitizedOutputs;
CxList outputsSocket = socket.DataInfluencingOn(outputsNotSanitized).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

outputs = outputRelevantMethods;
outputs.Add(outputsSocket); 
outputs.Add(outputsResponseParams);

//Anything that passes through the DB now has info from the DB and not the sensitive data
CxList sanitize = Find_DB_In();

//Add encryption as sanitizer
sanitize.Add(Find_Encrypt());

result = outputs.InfluencedByAndNotSanitized(personalInfo, sanitize).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);