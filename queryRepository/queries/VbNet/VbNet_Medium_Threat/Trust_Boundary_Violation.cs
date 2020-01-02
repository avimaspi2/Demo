CxList setAttr = All.FindByMemberAccess("Session.Add", false);
CxList sanitize = Find_Sanitize();
sanitize.Add(All.FindByShortName("Session", false));
CxList input = Find_Inputs();

result = setAttr.InfluencedByAndNotSanitized(input, sanitize);