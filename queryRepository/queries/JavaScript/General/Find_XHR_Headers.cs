// Look for XHR.setRequestHeader(header,value);

CxList wrappers = Find_XHR_Wrappers();

CxList XHROpen = Find_XmlHttp_Open();
XHROpen.Add(wrappers);

CxList XmlHttpRequest = All.FindAllReferences(XHROpen.GetAssignee());
XmlHttpRequest.Add(XHROpen);

result.Add(XmlHttpRequest.GetMembersOfTarget().FindByShortName("setRequestHeader"));

// Look for $.ajax({headers:values})
CxList XHRWrappers = wrappers.FindByShortNames(new List<string>{"ajax", "get", "getJSON", "post", "getScript"});
result.Add(All.GetParameters(XHRWrappers, 0));
CxList SAPXHRWrappers = Find_SAP_XHR_Wrappers();
CxList sjax = SAPXHRWrappers.FindByShortName("sjax");
result.Add(All.GetParameters(sjax, 0));
result.Add(All.GetParameters(SAPXHRWrappers - sjax, 1));