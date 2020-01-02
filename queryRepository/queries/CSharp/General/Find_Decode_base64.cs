CxList methods = Find_Methods();
result.Add(methods.FindByMemberAccess("Base64UrlTextEncoder.Decode", true));
result.Add(methods.FindByMemberAccess("WebEncoders.Base64UrlDecode", true));
result.Add(methods.FindByMemberAccess("Convert.FromBase64String", true));