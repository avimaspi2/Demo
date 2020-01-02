CxList methods = Find_Methods();

result.Add(methods.FindByMemberAccess("AntiXss.HtmlAttributeEncode", true));
result.Add(methods.FindByMemberAccess("AntiXss.HtmlEncode", true));
result.Add(methods.FindByMemberAccess("AntiXss.JavaScriptEncode", true));
result.Add(methods.FindByMemberAccess("AntiXss.UrlEncode", true));
result.Add(methods.FindByMemberAccess("AntiXss.VisualBasicScriptEncode", true));
result.Add(methods.FindByMemberAccess("AntiXss.XmlAttributeEncode", true));
result.Add(methods.FindByMemberAccess("AntiXss.XmlEncode", true));
result.Add(methods.FindByMemberAccess("HttpUtility.UrlEncodeUnicode", true));
result.Add(methods.FindByMemberAccess("HttpUtility.UrlEncodeUnicodeToBytes", true));