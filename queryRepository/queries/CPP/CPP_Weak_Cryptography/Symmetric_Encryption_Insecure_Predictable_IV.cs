//List of safe randoms
CxList sanitizers = Get_Variables_From_Addresses(Find_Safe_Randoms());

//List of memory copys
CxList memoryOverride = All.GetParameters(Find_Memory_Copy(), 0);

//List of iv parameters
CxList ivParams = Find_Cryptopp_IV_Parameters();
ivParams.Add(Find_OpenSSL_IV_Parameters());
ivParams.Add(Find_Botan_IV_Parameters());
ivParams = Get_Variables_From_Addresses(ivParams);

sanitizers = ivParams.InfluencedByAndNotSanitized(sanitizers, memoryOverride);

result.Add(Find_Symmetric_Encryption_Insecure_Predictable(sanitizers, ivParams));