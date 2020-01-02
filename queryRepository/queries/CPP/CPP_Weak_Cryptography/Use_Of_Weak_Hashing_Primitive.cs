/* This query tries to find when an application is using a weak hashing primitive:
	- MD2
	- MD4
	- MD5
	- RIPEMD-160
	- SHA-0
	- SHA-1
*/
CxList methods = Find_Methods();

//////////////////Crypto++////////////////
string[] weakHashAlgorithms = { "*MD2", "*MD4", "*MD5", "*RIPEMD160", "*SHA1"};
List < string > cryptoppMethods = new List<string> {
		"TruncatedFinal", "Final", "CalculateDigest", "CalculateTruncatedDigest"
		};

CxList weakHashObjects = All.FindByTypes(weakHashAlgorithms, true);
CxList cryptoppWeakFunctions = weakHashObjects.GetMembersOfTarget().FindByShortNames(cryptoppMethods, true);
result = cryptoppWeakFunctions;

/////////////////openSSL///////////////
// The method name already indicates what algorithm is being used
List < string > weakAlgorithmsMethods = new List<string> {
		"MD2", "MD4", "MD5", "RIPEMD160", "SHA1",
		"MD2_Final", "MD4_Final", "MD5_Final", "MDC2_Final", "RIPEMD160_Final", "SHA1_Final",
		"EVP_md2", "EVP_md4", "EVP_md5", "EVP_mdc2", "EVP_ripemd160", "EVP_sha1"
		};

result.Add(methods.FindByShortNames(weakAlgorithmsMethods));

// When EVP_get_digestbyname parameter is a weak algorithm
List < string > weakdigests = new List<string> { 
		"SHA1", "SHA1", "RIPEMD160", "MD2", "MD5" };

CxList opensslweakdigests = All.GetParameters(methods.FindByShortName("EVP_get_digestbyname"))
	.FindByShortNames(weakdigests);

result.Add(opensslweakdigests);

/////////////////Botan/////////////////
string[] botanWeakHash = {"MD4", "MD5", "SHA_160", "RIPEMD_160" };
weakHashObjects = All.FindByTypes(botanWeakHash, true);
CxList botanWeak = weakHashObjects.GetMembersOfTarget().FindByShortNames(
	new List<string> {"final", "process", "final_stdvec"});

result.Add(botanWeak);

// When a HashFunction is created using a weak algorithm as parameter
CxList createHashes = All.FindByType("HashFunction").GetMembersOfTarget().FindByShortNames(
	new List<string> { "create", "create_or_throw", "copy_state" });

CxList paramsCreate = All.GetParameters(createHashes).FindByShortNames(
	new List<string> { "SHA-1", "SHA-160", "SHA1", "RIPEMD-160", "MD5", "MD4" });

result.Add(paramsCreate);


///////////////Libsodium//////////////////
// If method crypto_generichash is called with with key = NULL or keylen = 0 then it will be 
// similar to MD5 or Sha - 1 wich are vulnerable
CxList weakMethod = methods.FindByShortName("crypto_generichash");

CxList nullKey = All.GetParameters(weakMethod, 4)
	.FindByAbstractValue(abstractValue => abstractValue is NullAbstractValue);

IAbstractValue zero = new IntegerIntervalAbstractValue(0);
CxList keyLengthZero = All.GetParameters(weakMethod, 5)
	.FindByAbstractValue(abstractValue => zero.IncludedIn(abstractValue));

result.Add(nullKey);
result.Add(keyLengthZero);