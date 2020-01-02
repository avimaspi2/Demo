if(param.Length > 0){

	CxList strings = (CxList) param[0];
	CxList methods = (CxList) param[1];
	CxList salt    = (CxList) param[2];
	
	////////////////////
	///Unix libcrypt///
	//////////////////

	string[] cryptDigestNames = { @"crypt", @"crypt_r"};

	CxList cryptDigest = methods.FindByShortNames(new List<string> (cryptDigestNames));

	result.Add(cryptDigest.InfluencedByAndNotSanitized(strings, salt));

}