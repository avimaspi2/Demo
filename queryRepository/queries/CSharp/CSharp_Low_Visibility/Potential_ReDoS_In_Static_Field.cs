CxList evilStrings = Find_Evil_Strings();

// Find all regex commands
CxList regex = Find_Regex() + Find_Inputs();

// Add static regexes (these do not influence their references, so needed in addition)
CxList stat = Find_Regex().FindByFieldAttributes(Modifiers.Static);
CxList staticFields = stat.FindByType(typeof(FieldDecl)) + stat.FindByType(typeof(ConstantDecl));

// Sanitization
CxList sanitize = Find_Sanitize();

result = evilStrings.InfluencingOnAndNotSanitized(regex.GetByAncs(staticFields), sanitize);
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);