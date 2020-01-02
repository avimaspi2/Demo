CxList jQuery = All.FindByShortNames(new List<string> {"jQuery", "$"});
CxList jQueryMethods = jQuery.GetMembersOfTarget();
CxList jQueryajaxWrapper = jQueryMethods.FindByShortNames(new List<string> {"ajax", "get", "getJSON", "post", "getScript"});

// Add methods that return XHR
result.Add(jQueryajaxWrapper);
List < string > XHRMethods = new List<string> {"done", "fail", "always", "success", "error", "complete"};
for (int i = 0; i < 5; i++)
{
	CxList methods = result.GetMembersOfTarget().FindByShortNames(XHRMethods);
	CxList assignee = methods.GetAssignee();
	result.Add(methods);
	result.Add(All.FindAllReferences(assignee));
}