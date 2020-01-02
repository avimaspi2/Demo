CxList methods = Find_Methods();

// Add general method names - the returned value is an integer
string[] methodGeneralNames = {@"SHA1_Init", @"MD2_Init", @"MD4_Init", @"MD5_Init", @"HMAC_Init", @"RIPEMD160_Init",
						@"SHA1_Update", @"MD2_Update", @"MD4_Update", @"MD5_Update", @"HMAC_Update", @"RIPEMD160_Update",
						@"HMAC_Cleanup", @"EVP_MD_CTX_init", @"EVP_DigestInit_ex", @"EVP_DigestUpdate", @"EVP_MD_CTX_cleanup",
						@"EVP_MD_CTX_destroy", @"EVP_MD_CTX_copy_ex", @"EVP_DigestInit", @"EVP_MD_CTX_copy",
						@"CC_SHA1_Init", @"CC_SHA1_Update", @"CC_SHA224_Init", @"CC_SHA224_Update", @"CC_SHA256_Init",
						@"CC_SHA256_Update", @"CC_SHA384_Init", @"CC_SHA384_Update", @"CC_SHA512_Init", @"CC_SHA512_Update",
						@"CC_MD2_Init", @"CC_MD2_Update", @"CC_MD4_Init", @"CC_MD4_Update", @"CC_MD5_Init", @"CC_MD5_Update"
						};

// Add macros - the returned value is an integer
string[] methodMacroNames = {@"EVP_MD_type", @"EVP_MD_pkey_type", @"EVP_MD_size", @"EVP_MD_block_size", @"EVP_MD_CTX_md",
						@"EVP_MD_CTX_size", @"EVP_MD_CTX_block_size", @"EVP_MD_CTX_type"};

// Add digest methods - the returned value is an encrypted string
string[] methodDigestNames = {@"SHA1", @"MD2", @"MD4", @"MD5", @"HMAC", @"RIPEMD160", @"CC_SHA1", @"CC_SHA224", @"CC_SHA256",
							@"CC_SHA384", @"CC_SHA512", @"CC_MD2", @"CC_MD4", @"CC_MD5", };

List<string> methodNames = new List<string>(methodGeneralNames);
methodNames.AddRange(methodMacroNames);
methodNames.AddRange(methodDigestNames);
result = methods.FindByShortNames(methodNames);

// Add final methods
string[] methodFinalNames = {@"SHA1_Final",@"MD2_Final", @"MD4_Final",@"MD5_Final",@"HMAC_Final", @"RIPEMD160_Final",
							@"CC_SHA1_Final",@"CC_SHA224_Final", @"CC_SHA256_Final",@"CC_SHA384_Final",@"CC_SHA512_Final",
							@"CC_MD2_Final", @"CC_MD4_Final", @"CC_MD5_Final"
							};
CxList methodFinal = methods.FindByShortNames(new List<string>(methodFinalNames));
result.Add(methodFinal);	// The returned value is integer
result.Add(All.GetParameters(methodFinal,0)); // The first parameter (buffer) containes the encrypted message

// Add CCHmac* methods - which do not return any value
result.Add(All.GetParameters(methods.FindByShortName("CCHmacFinal"), 1)); // The second parameter (buffer) containes the encrypted message
result.Add(All.GetParameters(methods.FindByShortName("CCHmac"), 5)); // The sixth parameter (buffer) containes the encrypted message

result.Add(Find_Integers());