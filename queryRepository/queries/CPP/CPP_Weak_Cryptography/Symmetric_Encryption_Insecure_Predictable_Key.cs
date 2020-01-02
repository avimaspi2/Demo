//Find all symmetric cipher keys that not are influenced by a secure random number generator
// 1st - Find all safe random generators as well functions that return a safe key
CxList sanitizers = Get_Variables_From_Addresses(Find_Safe_Randoms());
sanitizers.Add(Find_OpenSSL_Sanitizers_Key_Parameters());
sanitizers.Add(Find_Libsodium_Sanitizers_Key_Parameters());

//List of memory copys
CxList memoryOverride = All.GetParameters(Find_Memory_Copy(), 0);

// Find all key params
CxList keyParams = Find_Cryptopp_Key_Parameters();
keyParams.Add(Find_OpenSSL_Key_Parameters());
keyParams.Add(Find_Botan_Key_Parameters());
keyParams.Add(Find_Libsodium_Key_Parameters());
keyParams = Get_Variables_From_Addresses(keyParams);

// To avoid that a insecure key is copied to a variable that has been sanitized 
sanitizers = keyParams.InfluencedByAndNotSanitized(sanitizers, memoryOverride);

result = Find_Symmetric_Encryption_Insecure_Predictable(sanitizers, keyParams);