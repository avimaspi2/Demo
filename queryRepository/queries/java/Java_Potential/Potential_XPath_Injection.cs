CxList XPath = All.FindByMemberAccess("XPath.compile");		
XPath.Add(All.FindByMemberAccess("XPath.evaluate"));

CxList inputs = Find_Potential_Inputs();
CxList sanitized = Find_Sanitize();

result = XPath.InfluencedByAndNotSanitized(inputs, sanitized);