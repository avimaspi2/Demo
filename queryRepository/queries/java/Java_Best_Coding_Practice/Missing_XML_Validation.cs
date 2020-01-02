CxList parser = All.FindByTypes(new string [] {
	"SAXParserFactory",
	"DocumentBuilderFactory",
	"SAXReader"});

CxList setValidator = All.FindByMemberAccess("SAXParserFactory.setValidating");
setValidator.Add(All.FindByMemberAccess("DocumentBuilderFactory.setValidating"));
setValidator.Add(All.FindByMemberAccess("SAXReader.setValidation"));

CxList validator = setValidator.FindByParameters(All.FindByName("true")).GetTargetOfMembers();

CxList allResults = All.FindDefinition(parser);

allResults.Add(All.FindByShortName("newSAXParser").GetTargetOfMembers().FindByShortName("newInstance").
	GetTargetOfMembers().FindByShortName("SAXParserFactory"));

allResults -= All.FindDefinition(validator);


result.Add(allResults);