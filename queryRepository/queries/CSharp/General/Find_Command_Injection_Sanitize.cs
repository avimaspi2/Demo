CxList methods = Find_Methods();

result.Add(Find_Sanitize());
result.Add(Find_Encode());

//clean result list
CxList toRemove = All.NewCxList();
toRemove.Add(methods.FindByMemberAccess("AntiXss.HtmlAttributeEncode", true));
toRemove.Add(methods.FindByMemberAccess("AntiXss.HtmlEncode", true));
toRemove.Add(methods.FindByMemberAccess("AntiXss.XmlAttributeEncode", true));
toRemove.Add(methods.FindByMemberAccess("AntiXss.XmlEncode", true));
toRemove.Add(methods.FindByMemberAccess("Encoder.HtmlAttributeEncode", true));
toRemove.Add(methods.FindByMemberAccess("Encoder.HtmlEncode", true));
toRemove.Add(methods.FindByMemberAccess("Encoder.XmlEncode", true));
toRemove.Add(methods.FindByMemberAccess("Sanitizer.GetSafeHtml", true));
toRemove.Add(methods.FindByMemberAccess("Sanitizer.GetSafeHtmlFragment", true));
toRemove.Add(methods.FindByMemberAccess("SecurityElement.Escape", true));
toRemove.Add(methods.FindByMemberAccess("WebUtility.HtmlEncode", true));
toRemove.Add(methods.FindByName("HttpContext.Current.Server.HtmlEncode",true));
toRemove.Add(methods.FindByMemberAccess("HttpUtility.HtmlEncode", true));
toRemove.Add(methods.FindByMemberAccess("AntiXssEncoder.HtmlEncode", true));
toRemove.Add(methods.FindByMemberAccess("AntiXssEncoder.UrlEncode", true));
toRemove.Add(methods.FindByMemberAccess("AntiXssEncoder.XmlAttributesEncode", true));
toRemove.Add(methods.FindByMemberAccess("AntiXssEncoder.XmlEncode", true));
toRemove.Add(methods.FindByMemberAccess("Html.AttributeEncode", true));
toRemove.Add(methods.FindByMemberAccess("Html.Encode", true));
toRemove.Add(methods.FindByMemberAccess("Url.Encode", true));
result -= toRemove;