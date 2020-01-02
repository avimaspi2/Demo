CxList customAttributes = Find_CustomAttribute();
CxList cookieValueAnnotation = customAttributes.FindByCustomAttribute("CookieValue");

result = cookieValueAnnotation.GetFathers();