CxList xmlReader = All.FindByTypes(new string[]{"XmlTextReader","XmlReader"});
CxList xmlValidator = All.FindByTypes(new string[]{"XmlValidatingReader", "XmlSchema"});

result = All.FindDefinition(xmlReader) - All.FindDefinition(xmlReader.DataInfluencingOn(xmlValidator));