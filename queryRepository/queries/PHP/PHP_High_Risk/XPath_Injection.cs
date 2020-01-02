CxList methods = Find_Methods();

CxList inputs = Find_Interactive_Inputs();
CxList sanitized = Find_XPath_Sanitize();
// Methods which get a filter (possibly tainted) in their first patameter.
CxList XPath_parm1 = 
	methods.FindByMemberAccess("DOMXPath.evaluate") +
	methods.FindByMemberAccess("DOMXPath.query") + 
	methods.FindByMemberAccess("XPathContext.xpath_eval_expression") +
	methods.FindByMemberAccess("XPathContext.xpath_eval") +
	methods.FindByMemberAccess("XPathContext.xptr_eval") +
	methods.FindByMemberAccess("SimpleXMLElement.xpath");

CxList memberMethods = 
	/*methods.FindByShortName("evaluate") +*/
	/*methods.FindByShortName("query") +*/ 
	methods.FindByShortNames(new List<String>{ "xpath_eval_expression", "xpath_eval", "xptr_eval", "xpath" });
CxList unknownReferanceObj = All.FindByType(typeof(UnknownReference));
XPath_parm1.Add(unknownReferanceObj.GetMembersOfTarget() * memberMethods);

CxList tainted_parm1 = All.GetParameters(XPath_parm1, 0);
CxList other_params1 = All.GetParameters(XPath_parm1) - tainted_parm1;
other_params1.Add(XPath_parm1.GetTargetOfMembers());
result = XPath_parm1.InfluencedByAndNotSanitized(inputs, sanitized + other_params1);
	
// Methods which get a filter (possible tainted) in their second parameter.
// As some are stand-alone functions with same name as class methods of the previous group, 
// remove the instances of the first group members from the second group.
CxList XPath_parm2 = memberMethods + methods.FindByMemberAccess("SDO_DAS_Relational.executeQuery") 
	- XPath_parm1;

CxList tainted_parm2 = All.GetParameters(XPath_parm2, 1);
CxList other_params2 = All.GetParameters(XPath_parm2) - tainted_parm2;
other_params2.Add(XPath_parm2.GetTargetOfMembers());
result.Add(XPath_parm2.InfluencedByAndNotSanitized(inputs, sanitized + other_params2));