CxList methods = Find_Methods();
CxList allCryptoLibraries = Find_Require("crypto-js");
CxList crypto = Find_Require("crypto");
CxList unknownRefs = Find_UnknownReference();

//Crypto-JS
List <string> SHA_Methods = new List<string>() {"*md5", "*sha1", "*sha256", "*sha512", "*sha3", "*ripemd160"};
CxList specificCryptoLibrary = Find_Require("crypto-js*", 1, SHA_Methods) - allCryptoLibraries;
result.Add(specificCryptoLibrary);

List <string> HMAC_Methods = new List<string>() {"HmacMD5", "HmacSHA1", "HmacSHA256", "HmacSHA512"};
List <string> Ciphers_Methods = new List<string>() {"AES", "DES", "TripleDES", "Rabbit", "RC4", "RC4Drop"};

result.Add(allCryptoLibraries.GetMembersOfTarget().FindByShortNames(HMAC_Methods));
CxList ciphers = allCryptoLibraries.GetMembersOfTarget().FindByShortNames(Ciphers_Methods);
result.Add(ciphers.GetMembersOfTarget().FindByShortName("encrypt"));
CxList leftRef = All.FindByAssignmentSide(CxList.AssignmentSide.Left).GetByAncs(ciphers.GetFathers());
result.Add(All.FindAllReferences(leftRef).GetMembersOfTarget().FindByShortName("encrypt"));

//Crypto
List <string> Final_Methods = new List<string>() {"digest", "final", "read"};
CxList createMethods = crypto.GetMembersOfTarget().FindByShortName("create*");
CxList digestUse = methods.FindByShortNames(Final_Methods);
digestUse = createMethods.DataInfluencingOn(digestUse).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
result.Add(digestUse);

//Progressive Ciphering
CxList methodEnDescryptor = methods.FindByShortName("createEncryptor");
CxList elementsEnDesc = methodEnDescryptor.GetAssignee();

result.Add(unknownRefs.FindAllReferences(elementsEnDesc).GetMembersOfTarget().FindByShortName("process"));