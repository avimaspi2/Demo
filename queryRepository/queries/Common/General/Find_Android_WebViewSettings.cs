// Finds WebView settings references (ex webView.getSettings())

CxList urAndMembers = Find_UnknownReference();
urAndMembers.Add(Find_MemberAccesses());

List<String> sett = new List<String>() {"getSettings", "settings"};
CxList webSettings = Find_Android_WebViews().GetMembersOfTarget().FindByShortNames(sett);
webSettings.Add(urAndMembers.FindAllReferences(webSettings.GetAssignee()));

result = webSettings;