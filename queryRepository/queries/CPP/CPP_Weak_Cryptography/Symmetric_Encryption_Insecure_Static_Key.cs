CxList memcpyFirstParam = All.GetParameters(Find_Memcpy_Of_StringAbstractValues(), 0) - Find_Parameters();

//Libsodium
CxList sanitizers = Find_Libsodium_Sanitizers_Key_Parameters();
CxList keyParams = Find_Libsodium_Key_Parameters();
result.Add(Find_Symmetric_Encryption_Insecure_Static_Key(memcpyFirstParam, sanitizers, keyParams));

//OpenSSL
sanitizers = Find_OpenSSL_Sanitizers_Key_Parameters();
keyParams = Find_OpenSSL_Key_Parameters();
result.Add(Find_Symmetric_Encryption_Insecure_Static_Key(memcpyFirstParam, sanitizers, keyParams));

//Cryptopp
sanitizers = Find_Cryptopp_Sanitizers_Key_Parameters();
keyParams = Find_Cryptopp_Key_Parameters();
result.Add(Find_Symmetric_Encryption_Insecure_Static_Key(memcpyFirstParam, sanitizers, keyParams));

//Botan
sanitizers = Find_Botan_Sanitizers_Key_Parameters();
keyParams = Find_Botan_Key_Parameters();
result.Add(Find_Symmetric_Encryption_Insecure_Static_Key(memcpyFirstParam, sanitizers, keyParams));