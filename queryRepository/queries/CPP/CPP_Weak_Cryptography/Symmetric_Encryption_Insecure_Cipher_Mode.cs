/// <summary>
/// The following query will find Insecure Symmetric Encryption Cipher Mode.
/// </summary>

CxList insecureCipherMode = All.NewCxList();

// Symmetric_Encryption_Insecure_Cipher_Mode in Crypto++ (https://www.cryptopp.com)
insecureCipherMode.Add(Find_Cryptopp_Symmetric_Encryption_Insecure_Cipher_Mode());

// Symmetric_Encryption_Insecure_Cipher_Mode in OpenSSL (https://www.openssl.org)
insecureCipherMode.Add(Find_OpenSSL_Symmetric_Encryption_Insecure_Cipher_Mode());

// Symmetric_Encryption_Insecure_Cipher_Mode in Botan (https://botan.randombit.net)
insecureCipherMode.Add(Find_Botan_Symmetric_Encryption_Insecure_Cipher_Mode());

// Results (reduce flow)
result = insecureCipherMode.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);