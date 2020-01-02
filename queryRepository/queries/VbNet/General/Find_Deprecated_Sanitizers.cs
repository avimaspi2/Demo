CxList methods = Find_Methods();

result.Add(methods.FindByMemberAccess("AntiXss.HtmlAttributeEncode", false));
result.Add(methods.FindByMemberAccess("AntiXss.HtmlEncode", false));
result.Add(methods.FindByMemberAccess("AntiXss.JavaScriptEncode", false));
result.Add(methods.FindByMemberAccess("AntiXss.UrlEncode", false));
result.Add(methods.FindByMemberAccess("AntiXss.VisualBasicScriptEncode", false));
result.Add(methods.FindByMemberAccess("AntiXss.XmlAttributeEncode", false));
result.Add(methods.FindByMemberAccess("AntiXss.XmlEncode", false));
result.Add(methods.FindByMemberAccess("HttpUtility.UrlEncodeUnicode", false));
result.Add(methods.FindByMemberAccess("HttpUtility.UrlEncodeUnicodeToBytes", false));