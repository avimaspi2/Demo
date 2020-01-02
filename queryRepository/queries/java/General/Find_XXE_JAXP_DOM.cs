CxList methods = Find_Methods();

CxList xmlParseMthds = methods.FindByMemberAccess("DocumentBuilder.parse", false);
CxList sanitizers = Find_XXE_Sanitize();
CxList UNsanitizers = Find_XXE_UNSanitize();

result = xmlParseMthds - (xmlParseMthds.InfluencedBy(sanitizers)) + (xmlParseMthds.InfluencedBy(UNsanitizers));