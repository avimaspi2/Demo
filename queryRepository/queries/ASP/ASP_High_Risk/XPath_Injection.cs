CxList XPath = 
	Find_Member_With_Target("Microsoft.XMLDOM", "SelectSingleNode") + 
	Find_Member_With_Target("Msxml.DOMDocument*", "SelectSingleNode") + 
	Find_Member_With_Target("Msxml2.DOMDocument*", "SelectSingleNode") + 
	
	Find_Member_With_Target("Microsoft.XMLDOM", "SelectNodes") + 
	Find_Member_With_Target("Msxml.DOMDocument*", "SelectNodes") + 
	Find_Member_With_Target("Msxml2.DOMDocument*", "SelectNodes");
	
	
CxList docElement = 
	Find_Member_With_Target("Microsoft.XMLDOM", "DocumentElement") + 
	Find_Member_With_Target("Msxml.DOMDocument*", "DocumentElement") + 
	Find_Member_With_Target("Msxml2.DOMDocument*", "DocumentElement");

CxList docElementXPath = 
	docElement.GetMembersOfTarget().FindByShortName("SelectSingleNode", false) +
	docElement.GetMembersOfTarget().FindByShortName("SelectNodes", false);

XPath.Add(docElementXPath);

CxList inputs = Find_Interactive_Inputs();
CxList sanitized = Find_Sanitize();
result = XPath.InfluencedByAndNotSanitized(inputs, sanitized);