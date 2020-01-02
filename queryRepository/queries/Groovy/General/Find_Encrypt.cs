CxList encrypt = All.FindByShortName("*crypt*", false);
CxList encRemove = encrypt.FindByShortName("*decrypt*", false);
encRemove.Add(encrypt.FindByShortName("*unencrypt*", false));
encrypt -= encRemove;
encrypt.Add(All.FindByMemberAccess("Cipher.doFinal", false));
encrypt.Add(All.FindByMemberAccess("CipherInputStream.read", false));
encrypt.Add(All.FindByMemberAccess("CipherOutputStream.write", false));
encrypt.Add(All.FindByMemberAccess("MessageDigest.digest", false));

result = encrypt;