// We get all sensitive data. 
CxList personal_info = Find_Personal_Info();

// We remove strings, since they might contain: "Enter password".
// A potential problem  is that the might also contain: "password is....", but then it's hardcoded, 
// and not really sensitive information.
personal_info -= Find_Strings();
personal_info -= Find_Integers();

//remove declarators that are null or have an empty string assigned to it from personal_info
CxList nullOrEmpty = All.FindByName("null") + Find_Empty_Strings();
CxList assignedNull = nullOrEmpty.GetFathers() * personal_info;
assignedNull.Add(personal_info.FindByFathers(nullOrEmpty.GetFathers()));
personal_info -= assignedNull;

// We deal with 2 types of risky outputs - HttpServletResponse and Socket
CxList outputs = Find_Outputs();

// 1. HttpServletResponse must checked by HttpServletRequest.isSecure() if it is secured
CxList response = All.FindByType("HttpServletResponse");
CxList outputsResponse = outputs.DataInfluencedBy(response);
CxList isSecure = 
	Find_Conditions().FindByMemberAccess("HttpServletRequest.isSecure") +
	Find_Conditions().FindByMemberAccess("HTTPUtilties.isSecureChannel");
CxList secureIf = isSecure.GetFathers();
outputsResponse -= outputsResponse.GetByAncs(secureIf);

// 2. Socket is always insecure (should be SSLSocket to be secured)
CxList socket = All.FindByType("Socket");
CxList outputsSocket = outputs.DataInfluencedBy(socket);

outputs = outputsResponse + outputsSocket;

// Anyhing that passes through the DB now has info from the DB and not the sensitive data
CxList sanitize = Find_DB_In() + Find_Dead_Code_Contents();


result = outputs.InfluencedByAndNotSanitized(personal_info, sanitize);