CxList methods = Find_Methods();
result.Add(methods.FindByMemberAccess("Base64UrlTextEncoder.Encode", true));
result.Add(methods.FindByMemberAccess("WebEncoders.Base64UrlEncode", true));
result.Add(methods.FindByMemberAccess("Convert.ToBase64String", true));