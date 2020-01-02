CxList setAttr = All.FindByMemberAccess("session.add");
CxList sanitize = Find_Sanitize() + 
	All.FindByShortName("session");
CxList input = Find_Inputs();

result = setAttr.InfluencedByAndNotSanitized(input, sanitize);