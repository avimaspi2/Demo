CxList XPath = Find_XPath_Outputs();
CxList sanitized = Find_XPath_Sanitize();
CxList inputs = Find_Interactive_Inputs();

result = XPath.InfluencedByAndNotSanitized(inputs, sanitized);