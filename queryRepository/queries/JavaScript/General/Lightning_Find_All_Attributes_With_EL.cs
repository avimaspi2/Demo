cxXPath.AddSupportForExpressionLanguageForFramework("Lightning");
CxList AllAttributesThatHoldExpressions = cxXPath.FindAllAttributesThatHoldExpressions("*.cmp", 8, "Lightning");
AllAttributesThatHoldExpressions.Add(cxXPath.FindAllAttributesThatHoldExpressions("*.app", 8, "Lightning"));
result = AllAttributesThatHoldExpressions;