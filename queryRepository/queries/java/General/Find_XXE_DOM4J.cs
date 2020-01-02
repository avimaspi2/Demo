CxList methods = Find_Methods();

CxList xmlParseMthds = methods.FindByMemberAccess("DOMReader.read");
xmlParseMthds.Add(methods.FindByMemberAccess("SAXReader.read"));
xmlParseMthds.Add(methods.FindByMemberAccess("SAXModifier.modify"));
xmlParseMthds.Add(methods.FindByMemberAccess("XPP3Reader.read"));
xmlParseMthds.Add(methods.FindByMemberAccess("XPPReader.read"));

CxList sanitizers = Find_XXE_Sanitize();
CxList UNsanitizers = Find_XXE_UNSanitize();

result = xmlParseMthds - (xmlParseMthds.InfluencedBy(sanitizers)) + (xmlParseMthds.InfluencedBy(UNsanitizers));