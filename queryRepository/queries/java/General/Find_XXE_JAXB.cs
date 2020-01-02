CxList methods = Find_Methods();

CxList xmlParseMthds = methods.FindByMemberAccess("SAXBuilder.build");
xmlParseMthds.Add(methods.FindByMemberAccess("Unmarshaller.unmarshal"));
CxList sanitizers = Find_XXE_Sanitize();
CxList UNsanitizers = Find_XXE_UNSanitize();

result = xmlParseMthds - (xmlParseMthds.InfluencedBy(sanitizers)) + (xmlParseMthds.InfluencedBy(UNsanitizers));