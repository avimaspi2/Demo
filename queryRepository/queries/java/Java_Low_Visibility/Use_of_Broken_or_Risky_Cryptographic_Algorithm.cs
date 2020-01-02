CxList strings = Find_Strings();
CxList methods = Find_Methods();
CxList des = strings.FindByShortNames(new List<string>(){"*DES*", "*DESEDE*", "*TripleDES*", "*3DES*"}, false);

CxList cfmx = strings.FindByName("*CFMX_COMPAT*");
CxList rcX = strings.FindByShortNames(new List<string>(){"*RC2*", "*RC4*", "*RC5*", "*ARCFOUR*", "*Blowfish*"}, false);
rcX.Add(All.FindAllReferences(rcX.GetAssignee()));

CxList weakKeys = All.NewCxList();
weakKeys.Add(des);

weakKeys.Add(cfmx);

weakKeys.Add(rcX);

CxList KeyPairGenerator_initialize = All.FindByMemberAccess("KeyPairGenerator.initialize");

CxList getInstance = All.FindByMemberAccess("KeyGenerator.getInstance");
getInstance.Add(All.FindByMemberAccess("Cipher.getInstance"));
getInstance.Add(All.FindByMemberAccess("SecretKeyFactory.getInstance"));

var abstractValLessThan512 = new AbstractValueTypes.IntegerIntervalAbstractValue(null, 512);

CxList absValue = All.GetParameters(KeyPairGenerator_initialize).FindByAbstractValue(absVal => absVal.IncludedIn(abstractValLessThan512));
weakKeys.Add(absValue);

result.Add(getInstance.FindByParameters(weakKeys));
result.Add(KeyPairGenerator_initialize.FindByParameters(weakKeys));

//support MD5 MD2 MD4 SHA1 
CxList md5 = strings.FindByShortNames(new List<string> {
		"\"MD5\"",
		"\"MD2\"",
		"\"SHA-1\"",
		"\"MD4\""});

CxList messageDigestRet = methods.FindByMemberAccess("MessageDigest.getInstance");
result.Add(messageDigestRet.FindByParameters(md5));

//digesUtils
CxList digestUtilElements = methods.FindByMemberAccess("DigestUtils.*");
CxList digestElements = digestUtilElements.FindByShortNames(new List<string> {
		"md5*",
		"md2*",
		"sha1*"});
result.Add(digestElements);

//HMAC
CxList hmacs = methods.FindByMemberAccess("MAC.getInstance");
CxList hmacAlgorithms = strings.FindByShortNames(new List<string> {
		"\"HmacMD5\"",
		"\"HmacMD2\"",
		"\"HmacSHA-1\""});
result.Add(hmacs.FindByParameters(hmacAlgorithms));