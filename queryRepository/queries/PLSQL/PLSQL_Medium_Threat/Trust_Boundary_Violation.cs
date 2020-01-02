CxList setSessionAttr = 
	All.FindByMemberAccess("HTMLDB_UTIL.SET_SESSION_STATE", false) + 
	All.FindByMemberAccess("HTMLDB_UTIL.SET_PREFERENCE", false);

CxList sessionSanitize = Find_General_Sanitize();

CxList input = Find_Interactive_Inputs();

result = setSessionAttr.InfluencedByAndNotSanitized(input, sessionSanitize);