result.Add(All.FindByMemberAccess("XPathExpression.evaluate"));
CxList xpath = All.FindByMemberAccess("XPath.evaluate");
//Only 1st parameter of XPath.evaluate is problematic
result.Add(All.GetParameters(xpath, 0));

//xpath.compile(something).evaluate(input)
CxList compile = All.FindByMemberAccess("XPath.compile");
CxList members = compile.GetMembersOfTarget();
CxList list = members.FindByShortName("evaluate",false);
result.Add(list);