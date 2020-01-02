CxList strings = Find_Strings();
CxList methods = Find_Methods();

CxList sha = strings.FindByName("*SHA*") 
	- strings.FindByName("*SHA-2*")
	- strings.FindByName("*SHA-3*")
	- strings.FindByName("*SHA-5*")
	- strings.FindByName("*SHA2*")
	- strings.FindByName("*SHA3*")
	- strings.FindByName("*SHA5*");
CxList md5 = strings.FindByName("*MD5*");

CxList weakDigestUtils = All.FindByMemberAccess("DigestUtils.md5") 
	+ All.FindByMemberAccess("DigestUtils.md5Hex")
	+ All.FindByMemberAccess("DigestUtils.sha")
	+ All.FindByMemberAccess("DigestUtils.shaHex");

CxList weakKeys = md5 + sha;

CxList getInstance = methods.FindByShortName("getInstance");


result = getInstance.FindByParameters(weakKeys) + weakDigestUtils;