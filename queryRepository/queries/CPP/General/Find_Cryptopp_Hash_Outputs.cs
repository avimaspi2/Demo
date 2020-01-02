string[] hashAlgorithms = {
	"SHA3*", "Keccak*", "TIGER", "Whirlpool", "MD2", 
	"SHA512", "SHA224", "SHA256", "SHA1", "SM3", 
	"RIPEMD160", "RIPEMD-320", "RIPEMD128", "RIPEMD-256", "SipHash",  "MD4", "MD5"
	};

List < string > methods = new List<string> {
		"TruncatedFinal", "Final", "CalculateDigest", "CalculateTruncatedDigest"
		};

CxList hashObjects = All.FindByTypes(hashAlgorithms,true);
CxList hashFunctions = hashObjects.GetMembersOfTarget();
hashFunctions = hashFunctions.FindByShortNames(methods,true);

result = All.GetParameters(hashFunctions, 0);