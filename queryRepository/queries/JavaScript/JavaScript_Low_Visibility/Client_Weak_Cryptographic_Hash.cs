// Using md5 or sha1 is not secure
CxList objectCreations = Find_ObjectCreations();

CxList methods = Find_Methods() - XS_Find_All();

CxList cryptoMethods = methods.FindByShortName("md5*", false); 
cryptoMethods.Add(methods.FindByShortName("sha1*", false));
cryptoMethods.Add(Find_Members("CryptoJS.MD5").FindByType(typeof(MethodInvokeExpr)));
cryptoMethods.Add(Find_Members("CryptoJS.SHA1").FindByType(typeof(MethodInvokeExpr)));

// Typescript Cryptography Toolkit

CxList SHACreate = methods.FindByMemberAccess("SHA.create");
SHACreate.Add(objectCreations.FindByShortName("SHA"));

CxList variant = All.GetParameters(SHACreate, 0);

CxList jsSha = objectCreations.FindByShortName("jsSHA");
jsSha.Add(All.FindAllReferences(jsSha.GetAssignee()));

CxList jsShaGetHash = jsSha.GetMembersOfTarget().FindByShortName("getHash");
variant.Add(All.GetParameters(jsShaGetHash, 0));

CxList SHA1String = Find_String_Literal().FindByShortName("SHA-1");
SHA1String.Add(SHA1String.GetAssignee());

result = cryptoMethods;
result.Add(variant.FindAllReferences(SHA1String));