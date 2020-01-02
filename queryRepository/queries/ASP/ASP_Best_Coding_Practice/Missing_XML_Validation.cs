CxList xmlReader = All.FindByType("xmltextreader");
CxList xmlValidator = All.FindByType("xmlvalidatingreader") + All.FindByType("xmlschema");

result = All.FindDefinition(xmlReader)  - All.FindDefinition(xmlReader.DataInfluencingOn(xmlValidator));