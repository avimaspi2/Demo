CxList methods = Find_Methods();
CxList db = Find_DB_Out() + Find_Read();
CxList sanitized = Find_XPath_Sanitize();

CxList xpath = methods.FindByShortName("xpath_*",false);
CxList xpathEval=xpath.FindByShortName("xpath_eval*",false);
CxList exptr = methods.FindByShortName("xptr_*", false);

// Methods which get a filter (possibly tainted) in their first patameter.
CxList XPath_parm1 = methods.FindByMemberAccess("DOMXPath.evaluate");
XPath_parm1.Add(methods.FindByMemberAccess("DOMXPath.query"));
XPath_parm1.Add(xpathEval.FindByMemberAccess("XPathContext.xpath_eval_expression"));
XPath_parm1.Add(xpathEval.FindByMemberAccess("XPathContext.xpath_eval"));
XPath_parm1.Add(exptr.FindByMemberAccess("XPathContext.xptr_eval"));
XPath_parm1.Add(xpath.FindByMemberAccess("SimpleXMLElement.xpath"));

CxList memberMethods = xpathEval.FindByShortNames(new List<string>{ "xpath_eval_expression", "xpath_eval" });
memberMethods.Add(exptr.FindByShortName("xptr_eval"));
memberMethods.Add(xpath.FindByShortName("xpath"));

XPath_parm1.Add(memberMethods.GetMembersOfTarget());

CxList tainted_parm1 = All.GetParameters(XPath_parm1, 0);
CxList other_params1 = All.GetParameters(XPath_parm1) - tainted_parm1;
other_params1.Add(XPath_parm1.GetTargetOfMembers());
result = XPath_parm1.InfluencedByAndNotSanitized(db, sanitized + other_params1);
	
// Methods which get a filter (possible tainted) in their second parameter.
// As some are stand-alone functions with same name as class methods of the previous group, 
// remove the instances of the first group members from the second group.
CxList XPath_parm2 =  memberMethods+
	methods.FindByMemberAccess("SDO_DAS_Relational.executeQuery") -
	XPath_parm1;

CxList tainted_parm2 = All.GetParameters(XPath_parm2, 1);
CxList other_params2 = All.GetParameters(XPath_parm2) - tainted_parm2;
other_params2.Add(XPath_parm2.GetTargetOfMembers());
result.Add(XPath_parm2.InfluencedByAndNotSanitized(db, sanitized + other_params2));