CxList methods = Find_Methods();

CxList xml_parse_mthds = methods.FindByMemberAccess("XmlReader.Create");

//CxList sanitizers = Find_XXE_Sanitize();
CxList UNsanitizers = Find_XXE_UNSanitize();

result = xml_parse_mthds.InfluencedBy(UNsanitizers);