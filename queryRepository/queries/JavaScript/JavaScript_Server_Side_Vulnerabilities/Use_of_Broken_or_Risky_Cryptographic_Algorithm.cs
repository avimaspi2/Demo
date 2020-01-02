CxList methodInvoke = Find_Methods();
CxList strings = NodeJS_Find_Strings();
CxList cryptoRequires = Find_Require("crypto");

CxList allInfluByRequireCrypto = methodInvoke.DataInfluencedBy(cryptoRequires);
//All methodInvoke influenced by required library
CxList methInvFromRequire = allInfluByRequireCrypto * methodInvoke;


//PBKDF2 applies pseudorandom function HMAC-SHA1 to derive a key of given length from the given password, salt and iterations
CxList pbkdf2Meth = methInvFromRequire.FindByMemberAccess("*.pbkdf2");
pbkdf2Meth.Add(methInvFromRequire.FindByMemberAccess("*.pbkdf2Sync"));

//support MD5 MD2 MD4 SHA1 
CxList hashStrings = strings.FindByShortNames(
	new List<string> {
		"MD5", "MD2", "MD4", "SHA1"
	},
	false
);

CxList cryptoMethods = methInvFromRequire.FindByMemberAccess("*.createCipher");
cryptoMethods.Add(methInvFromRequire.FindByMemberAccess("*.createCipheriv"));
cryptoMethods.Add(methInvFromRequire.FindByMemberAccess("*.createDecipher"));
cryptoMethods.Add(methInvFromRequire.FindByMemberAccess("*.createDecipheriv"));
cryptoMethods.Add(methInvFromRequire.FindByMemberAccess("*.createHash"));
cryptoMethods.Add(methInvFromRequire.FindByMemberAccess("*.createHmac"));
cryptoMethods.Add(methInvFromRequire.FindByMemberAccess("*.createSign"));
cryptoMethods.Add(methInvFromRequire.FindByMemberAccess("*.createVerify"));

result.Add(cryptoMethods.DataInfluencedBy(hashStrings));
result.Add(pbkdf2Meth);