CxList encrypter = All.FindByShortName("*crypt*", false); // crypt is a PHP function used to encrypt strings, and all variables labelled crypt(ed) are considered safe, as well as DBMS_CRYPTO as output
encrypter -= encrypter.FindByShortName("*decrypt*");
CxList encrypterMethods = Find_ObjectCreations().FindByShortName("SecureString");
encrypterMethods.Add(All.FindByMemberAccess("CryptoStream.Write*"));
encrypter.Add(All.FindByFathers(encrypterMethods));

result = encrypter;