CxList methods = Find_Methods();
	
//Case 1:  hash is the first parameter
List<string> functionNamesCase1 = new List<string>() {
		"crypto_generichash",
		"crypto_pwhash_str", 
		"crypto_pwhash",
		"crypto_shorthash",
		"crypto_hash_sha256",
		"crypto_auth_hmacsha256",
		"crypto_hash_sha512", 
		"crypto_auth_hmacsha512", 
		"crypto_auth_hmacsha512256",
		"crypto_pwhash_scryptsalsa208sha256", "crypto_pwhash_scryptsalsa208sha256_str" 		
		};

CxList functionsCase1 = methods.FindByShortNames(functionNamesCase1);
result.Add(All.GetParameters(functionsCase1,0));

//Case 2:  hash is the second parameter
List<string> functionNamesCase2 = new List<string>() {
		"crypto_generichash_final",
		"crypto_hash_sha256_final",
		"crypto_auth_hmacsha256_final",
		"crypto_hash_sha512_final",
		"crypto_auth_hmacsha512_final",
		"crypto_auth_hmacsha512256_final",
		};

CxList functionsCase2 = methods.FindByShortNames(functionNamesCase2);
result.Add(All.GetParameters(functionsCase2, 1));