CxList methods = Find_Methods();
CxList webViews = methods.FindByShortName("loadData:MIMEType:textEncodingName:baseURL:");
webViews.Add(methods.FindByMemberAccess("UIWebView.loadHTMLString:baseURL:"));
webViews.Add(methods.FindByMemberAccess("UIWebView.loadRequest:"));
webViews.Add(methods.FindByMemberAccess("UIWebView.stringByEvaluatingJavaScriptFromString:"));
result = All.GetParameters(webViews, 0);