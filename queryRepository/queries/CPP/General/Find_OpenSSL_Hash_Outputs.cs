CxList methods = Find_Methods();

//Case 1:  hash is the first parameter
List < string > functionNamesCase1 = new List<string> {
		"MD2_Final", "MD4_Final", "MD5_Final", "MDC2_Final", "RIPEMD160_Final", 
		"SHA1_Final", "SHA224_Final", "SHA256_Final", "SHA384_Final", "SHA512_Final"
		};

CxList hashCase1 = methods.FindByShortNames(functionNamesCase1);
result.Add(All.GetParameters(hashCase1, 0));


//Case 2: hash is the second parameter
List <string> functionNamesCase2 = new List<string> {
		"HMAC_Final"
		};
CxList hashCase2 = methods.FindByShortNames(functionNamesCase2);
result.Add(All.GetParameters(hashCase2, 1));

//Case 3: hash is the third parameter
List <string> functionNamesCase3 = new List<string> {
		"MD5", "MD4", "MD2", "MDC2", "RIPEMD160", "WHIRLPOOL",
		"SHA1", "SHA", "SHA224", "SHA256", "SHA384", "SHA512"
		};
CxList hashCase3 = methods.FindByShortNames(functionNamesCase3);
result.Add(All.GetParameters(hashCase3, 2));

//Case 4: hash is the sixth parameter
List <string> functionNamesCase4 = new List<string> {
		"HMAC"
		};
CxList hashCase4 = methods.FindByShortNames(functionNamesCase4);
result.Add(All.GetParameters(hashCase4, 5));

//Case 5: hash is the return value
List < string > functionNamesCase5 = new List<string> {
		"EVP_blake2*",  
		"EVP_md2", 
		"EVP_md4", 
		"EVP_md5", 
		"EVP_mdc2", 
		"EVP_ripemd160", 
		"EVP_sha1", 
		"EVP_sha224", 
		"EVP_sha256", 
		"EVP_sha384", 
		"EVP_sha3_*",  
		"EVP_sha512*", 
		"EVP_shake*", 
		"EVP_sm3", 
		"EVP_whirlpool"
		};
result.Add(methods.FindByShortNames(functionNamesCase5));