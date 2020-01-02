int saltSize = 32;

if(param.Length == 1 && param[0] is int)
{
	saltSize = (int) param[0];
}

	// Scrypt_Weak_Salt_Value in OpenSSL
	CxList openSSLResults = All.NewCxList();
	openSSLResults.Add(Find_OpenSSL_PBKDF2_Weak_Salt_Size(saltSize));
	openSSLResults.Add(Find_OpenSSL_PBKDF2_Weak_Salt_Randomization());

	// Scrypt_Weak_Salt_Value in Botan
	CxList botanResults = All.NewCxList();
	botanResults.Add(Find_Botan_PBDKF2_Weak_Salt_Size(saltSize));
	botanResults.Add(Find_Botan_PBDKF2_Weak_Salt_Randomization());

	// Scrypt_Weak_Salt_Value in Cryptopp
	CxList cryptoppResults = All.NewCxList();
	cryptoppResults.Add(Find_Cryptopp_PBKDF2_Weak_Salt_Size(saltSize));
	cryptoppResults.Add(Find_Cryptopp_PBKDF2_Weak_Salt_Randomization());

	// Results
	result = openSSLResults.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);
	result.Add(botanResults);
	result.Add(cryptoppResults);