cxXPath.AddSupportForExpressionLanguageForFramework("Lightning");
CxList attributeDeclarations = cxXPath.createAttributesDefinition("*.cmp", 8, "Lightning");
attributeDeclarations.Add(cxXPath.createAttributesDefinition("*.app", 8, "Lightning"));
result = attributeDeclarations;