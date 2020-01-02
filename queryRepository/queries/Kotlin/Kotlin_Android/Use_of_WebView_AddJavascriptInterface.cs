CxList methods = Find_Methods();
CxList members = Find_MemberAccesses();

CxList webviews = methods.FindByMemberAccess("WebView.addJavascriptInterface");

CxList setEnable = methods.FindByShortName("setJavaScriptEnabled");
CxList enable = All.GetParameters(setEnable);
enable.Add(members.FindByMemberAccess("WebSettings.javaScriptEnabled").GetAssigner());

CxList enableTrue = enable.FindByAbstractValue(abstractValue => abstractValue is TrueAbstractValue);

CxList attribute = All.FindByCustomAttribute("JavascriptInterface");

result = webviews;
result.Add(enableTrue);
result.Add(attribute);