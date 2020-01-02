CxList methods = Find_Methods();
CxList strings = Find_String_Literal();

CxList suspects = strings.FindByShortName("*jsonp*");
suspects.Add(strings.FindByShortName("*callback=*"));
suspects.Add(strings.FindByShortName("*accept=jsonp*"));
suspects.Add(strings.FindByShortName("*format=json2*"));
suspects.Add(strings.FindByShortName("*callbackParam*"));
suspects.Add(Find_Inputs());

CxList callers = All.NewCxList();
callers.Add(methods.FindByMemberAccess("*.ajax"));
callers.Add(methods.FindByMemberAccess("jQuery.ajax"));
callers.Add(methods.FindByMemberAccess("*.getJSON"));
callers.Add(methods.FindByMemberAccess("jQuery.getJSON"));
callers.Add(methods.FindByMemberAccess("$jsonp.send"));
callers.Add(methods.FindByMemberAccess("XMLHttpRequest.open"));

// code for IE6, IE5
CxList activeXObject = methods.FindByMemberAccess("ActiveXObject.open");
activeXObject = activeXObject.DataInfluencedBy(strings.FindByShortName("Microsoft.XMLHTTP"));
callers.Add(activeXObject.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly));

result = callers.DataInfluencedBy(suspects);