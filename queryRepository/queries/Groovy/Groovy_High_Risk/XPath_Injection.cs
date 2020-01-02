CxList XPath = Find_XPath_Outputs();
CxList inputs = Find_Interactive_Inputs();
CxList sanitized = Find_Sanitize() + Get_ESAPI().FindByMemberAccess("Encoder.encodeForXPath");

result = XPath.InfluencedByAndNotSanitized(inputs, sanitized);