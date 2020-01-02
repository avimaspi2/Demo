CxList methods = Find_Methods();

CxList xml_parse_mthds = methods.FindByMemberAccess("XMLReader.parse");
xml_parse_mthds.Add(methods.FindByMemberAccess("SAXParserFactory.newSAXParser").
		GetMembersOfTarget().FindByName("*.getXMLReader").
			GetMembersOfTarget().FindByName("parse"));
	
CxList sanitizers = Find_XXE_Sanitize();
CxList UNsanitizers = Find_XXE_UNSanitize();

CxList allMethods = All.NewCxList();
allMethods.Add(xml_parse_mthds.InfluencedBy(sanitizers));	
allMethods.Add(xml_parse_mthds.InfluencedBy(UNsanitizers));	

result = xml_parse_mthds - allMethods;