CxList xmlReader = All.FindByType("XmlTextReader", false);
CxList xmlValidator = All.FindByType("XmlValidatingReader", false);
xmlValidator.Add(All.FindByType("XmlSchema", false));

result = All.FindDefinition(xmlReader)  - All.FindDefinition(xmlReader.DataInfluencingOn(xmlValidator));