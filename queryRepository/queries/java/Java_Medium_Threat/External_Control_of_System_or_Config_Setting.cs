CxList setters = Find_Setters();

CxList sanitize = Find_General_Sanitize();
sanitize.Add(setters.GetTargetOfMembers());
CxList inputs = Find_Interactive_Inputs();

result = inputs.InfluencingOnAndNotSanitized(setters, sanitize);