CxList XPath = Find_XPath_Output();

CxList inputs = Find_Interactive_Inputs();
CxList sanitized = Find_Sanitize();

result = XPath.InfluencedByAndNotSanitized(inputs, sanitized);