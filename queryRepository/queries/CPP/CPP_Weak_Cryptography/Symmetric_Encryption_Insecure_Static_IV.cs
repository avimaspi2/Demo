CxList memcpyFirstParam = All.GetParameters(Find_Memcpy_Of_StringAbstractValues(), 0) - Find_Parameters();

//Gets a list of safe randoms
CxList sanitizers = Get_Variables_From_Addresses(Find_Safe_Randoms());
 
CxList ivParams = Find_OpenSSL_IV_Parameters();
ivParams.Add(Find_Cryptopp_IV_Parameters());
ivParams.Add(Find_Botan_IV_Parameters());

//OpenSSL
result.Add(Find_Symmetric_Encryption_Insecure_Static_IV(memcpyFirstParam, sanitizers, ivParams));

//Cryptopp
result.Add(Find_Symmetric_Encryption_Insecure_Static_IV(memcpyFirstParam, sanitizers, ivParams));

//Botan
result.Add(Find_Symmetric_Encryption_Insecure_Static_IV(memcpyFirstParam, sanitizers, ivParams));