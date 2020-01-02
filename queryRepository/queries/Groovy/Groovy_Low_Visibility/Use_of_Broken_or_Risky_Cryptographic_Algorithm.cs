CxList strings = Find_Strings();
CxList methods = Find_Methods();
CxList des = strings.FindByName("*DES*")
	- strings.FindByName("*DESEDE*")
	- strings.FindByName("*TripleDES*");

CxList cfmx = strings.FindByName("*CFMX_COMPAT*");
CxList weakKeys = des + cfmx;

CxList getInstance = All.FindByMemberAccess("KeyGenerator.getInstance")
	+ All.FindByMemberAccess("Cipher.getInstance");


result = getInstance.FindByParameters(weakKeys);





//support MD5 MD2 MD4 SHA1 
CxList md5 = strings.FindByShortName("\"MD5\"") + strings.FindByShortName("\"MD2\"") + strings.FindByShortName("\"SHA-1\"") +
	strings.FindByShortName("\"MD4\"");
CxList messageDigestRet = methods.FindByMemberAccess("MessageDigest.getInstance");
result.Add(messageDigestRet.FindByParameters(md5));
//digesUtils
CxList digestUtilElements = methods.FindByMemberAccess("DigestUtils.*");
result.Add(digestUtilElements.FindByShortName("md5*") + digestUtilElements.FindByShortName("md2*") +
	digestUtilElements.FindByShortName("sha1*"));

//HMAC
CxList hmacs = methods.FindByMemberAccess("MAC.getInstance");
CxList hmacAlgorithms = strings.FindByShortName("\"HmacMD5\"") + strings.FindByShortName("\"HmacMD2\"") + strings.FindByShortName("\"HmacSHA-1\"");