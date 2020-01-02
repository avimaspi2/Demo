if(param.Length > 0){

	CxList strings = (CxList) param[0];
	CxList methods = (CxList) param[1];
	CxList salt	   = (CxList) param[2];
	
	//////////////
	///openSSL///
	////////////
	//
	//openSSL provides 2 ways of encrypting data:
	//MultiStep  -> Several functions that prepare the data to be encrypted, and the encryption can be divided in several steps
	//SingleStep -> One function, that receives the data and returns the encrypted data.
	///////////


 
	//1 MultiStep
	/////////////

	//Init Methods 
	string[] methodGeneralNames = {@"SHA1_Init", @"SHA256_Init" ,
		@"MD2_Init", @"MD4_Init", @"MD5_Init", @"HMAC_Init", @"RIPEMD160_Init",
		@"EVP_MD_CTX_init", @"EVP_DigestInit_ex", @"EVP_DigestInit",
		@"CC_SHA1_Init", @"CC_SHA224_Init", @"CC_SHA256_Init",
		@"CC_SHA384_Init", @"CC_SHA512_Init",
		@"CC_MD2_Init", @"CC_MD4_Init", @"CC_MD5_Init"};


	//Find init methods
	CxList genMethods = methods.FindByShortNames(new List<string> (methodGeneralNames));


	//Update methods
	string[] updateNames = {@"SHA1_Update",@"SHA256_Update", @"MD2_Update", @"MD4_Update", @"MD5_Update", @"HMAC_Update", @"RIPEMD160_Update",
		@"CC_SHA1_Update", @"CC_SHA224_Update", @"CC_SHA256_Update", @"CC_SHA384_Update", @"CC_SHA512_Update", @"CC_MD4_Update",
		@"CC_MD5_Update", @"EVP_DigestUpdate"};

	//Find udpate methods
	CxList updateMethods = methods.FindByShortNames(new List<string> (updateNames));


	// Add final methods
	string[] methodFinalNames = {@"SHA1_Final", @"SHA256_Final",@"MD2_Final", @"MD4_Final",@"MD5_Final",@"HMAC_Final", @"RIPEMD160_Final",
		@"CC_SHA1_Final",@"CC_SHA224_Final", @"CC_SHA256_Final",@"CC_SHA384_Final",@"CC_SHA512_Final",
		@"CC_MD2_Final", @"CC_MD4_Final", @"CC_MD5_Final"
		};

	//Find final methods
	CxList finalMethods = methods.FindByShortNames(new List<string> (methodFinalNames));


	//Get the parameters of update methods
	CxList updParam = All.GetParameters(updateMethods).FindByType(typeof(UnaryExpr));

	//Get the init methods of update methods
	CxList genParam = All.GetParameters(genMethods).FindByType(typeof(UnaryExpr));


	foreach(CxList gparam in genParam)
	{	//See which parameters from init methods, are influencing update methods
		CxList sanitized = gparam.InfluencingOn(updParam);	
		CxList endNodes = sanitized.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly); //Get update nodes
		if(endNodes.Count > 1) //If exist's more than two update nodes, then the final methods are salted.
			genParam -= gparam;//So they can be removed from the init list
	
	}
	//Get end nodes influeced by salt that are Methods
	CxList san = salt.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly).FindByType(typeof(MethodInvokeExpr));

	//Get the parameters of the methods that are salted and are update methods
	CxList salt_param = All.GetParameters(san * updateMethods);

	//Remove init methods that are influencing salted update methods
	genParam -= genParam.InfluencingOn(salt_param);

	//Find init methods that are influencing final methods and are not sanitized by salt
	result.Add(finalMethods.InfluencedByAndNotSanitized(genParam, salt));

	//2 SingleStep
	//////////////

	// Add digest methods - the returned value is an encrypted string
	string[] methodDigestNames = {@"SHA1", @"MD2", @"MD4", @"MD5", @"HMAC", @"RIPEMD160", @"CC_SHA1", @"CC_SHA224", @"CC_SHA256",
		@"CC_SHA384", @"CC_SHA512", @"CC_MD2", @"CC_MD4", @"CC_MD5"};

	//Find digest methods
	CxList digestMethods = methods.FindByShortNames(new List<string> (methodDigestNames));

	//Find digest methods that are influenced by strings and not sanitized
	result.Add(digestMethods.InfluencedByAndNotSanitized(strings, salt));
	
}