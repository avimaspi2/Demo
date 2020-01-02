CxList encrypt = All.FindByShortName("*encrypt*", false);
encrypt.Add(All.FindByMemberAccess("ICryptoTransform.TransformBlock", false));
encrypt.Add(All.FindByMemberAccess("ICryptoTransform.TransformFinalBlock", false));

encrypt.Add(All.FindByMemberAccess("*.CreateEncryptor",false));
encrypt.Add(All.FindByMemberAccess("CryptoStream.Write",false));
encrypt.Add(All.FindByMemberAccess("CryptoStream.BeginWrite",false));
encrypt.Add(All.FindByMemberAccess("DSA*.CreateSignature", false));
encrypt.Add(All.FindByMemberAccess("RSA*.Encrypt*",false));
encrypt.Add(All.FindByMemberAccess("MD5*.ComputeHash", false));
encrypt.Add(All.FindByMemberAccess("SHA*.ComputeHash", false));
encrypt.Add(All.FindByMemberAccess("HMA*.ComputeHash", false));
encrypt.Add(All.FindByMemberAccess("ProtectedData.Protect", false));

result = encrypt;