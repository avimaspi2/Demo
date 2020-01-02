CxList methods = Find_Methods();

CxList xml_parse_mthds = 
	methods.FindByMemberAccess("XmlDocument.Load");
xml_parse_mthds.Add(methods.FindByMemberAccess("XmlDocument.LoadXml"));

CxList sanitizers = Find_XXE_Sanitize();
CxList UNsanitizers = Find_XXE_UNSanitize();

result.Add(xml_parse_mthds);
result -= xml_parse_mthds.DataInfluencedBy(sanitizers);
result.Add(xml_parse_mthds.DataInfluencedBy(UNsanitizers));