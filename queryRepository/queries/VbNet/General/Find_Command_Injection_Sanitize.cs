CxList methods = Find_Methods();
result.Add(Find_Encode());

//clean result list
CxList toRemove = All.NewCxList();
toRemove.Add(methods.FindByMemberAccess("AntiXss.HtmlAttributeEncode", false));
toRemove.Add(methods.FindByMemberAccess("AntiXss.HtmlEncode", false));
toRemove.Add(methods.FindByMemberAccess("AntiXss.XmlAttributeEncode", false));
toRemove.Add(methods.FindByMemberAccess("AntiXss.XmlEncode", false));
toRemove.Add(methods.FindByMemberAccess("Encoder.HtmlAttributeEncode", false));
toRemove.Add(methods.FindByMemberAccess("Encoder.HtmlEncode", false));
toRemove.Add(methods.FindByMemberAccess("Encoder.XmlEncode", false));
toRemove.Add(methods.FindByMemberAccess("Sanitizer.GetSafeHtml", false));
toRemove.Add(methods.FindByMemberAccess("Sanitizer.GetSafeHtmlFragment", false));
toRemove.Add(methods.FindByMemberAccess("SecurityElement.Escape", false));
toRemove.Add(methods.FindByMemberAccess("WebUtility.HtmlEncode", false));
toRemove.Add(methods.FindByName("HttpContext.Current.Server.HtmlEncode", false));
toRemove.Add(methods.FindByMemberAccess("HttpUtility.HtmlEncode", false));
toRemove.Add(methods.FindByMemberAccess("AntiXssEncoder.HtmlEncode", false));
toRemove.Add(methods.FindByMemberAccess("AntiXssEncoder.UrlEncode", false));
toRemove.Add(methods.FindByMemberAccess("AntiXssEncoder.XmlAttributesEncode", false));
toRemove.Add(methods.FindByMemberAccess("AntiXssEncoder.XmlEncode", false));
toRemove.Add(methods.FindByMemberAccess("Html.AttributeEncode", false));
toRemove.Add(methods.FindByMemberAccess("Html.Encode", false));
toRemove.Add(methods.FindByMemberAccess("Url.Encode", false));
result -= toRemove;