/**
 	This query finds instances of PKCS#1v1.5 padding in RSA.
*/

CxList methods = Find_Methods();
CxList objcreates = Find_ObjectCreations();
CxList strings = Find_String_Literal();
CxList unkrefs = Find_Unknown_References();

// Crypto++

CxList cryptoPPVulnerabilities = All.NewCxList();
cryptoPPVulnerabilities.Add(methods.FindByMemberAccess("RSASSA_PKCS1v15_SHA_Signer.SignMessage"));
cryptoPPVulnerabilities.Add(methods.FindByMemberAccess("RSASSA_PKCS1v15_SHA_Verifier.VerifyMessage"));

// Botan

CxList botanEMEPKCS1v15 = strings.FindByShortName("EME-PKCS1-v1_5");
CxList botanPKEncryptorEME = objcreates.FindByShortName("PK_Encryptor_EME");
CxList botanPKDecryptorEME = objcreates.FindByShortName("PK_Decryptor_EME");
CxList botanVulnerabilities = botanEMEPKCS1v15.InfluencingOn(botanPKEncryptorEME + botanPKDecryptorEME);

// OpenSSL

CxList openSSLRSAPKCS1Padding = unkrefs.FindByShortName("RSA_PKCS1_PADDING");
CxList openSSLRSARelevantMethods = methods.FindByShortNames(new List<string>{
		"RSA_public_encrypt", 
		"RSA_private_decrypt",
		"RSA_padding_check_PKCS1_type_2"});

CxList openSSLEVPRelevantMethods = methods.FindByShortNames(new List<string>{
		"EVP_PKEY_decrypt",
		"EVP_PKEY_verify_recover"});

CxList openSSLEVPSetPadding = methods.FindByShortName("EVP_PKEY_CTX_set_rsa_padding");
CxList openSSLVulnerableEVPSetPadding = openSSLRSAPKCS1Padding.InfluencingOn(openSSLEVPSetPadding).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
CxList openSSLVulnerableEVPCtx = unkrefs.GetParameters(openSSLVulnerableEVPSetPadding, 0);

CxList openSSLVulnerabilities = All.NewCxList();
openSSLVulnerabilities.Add(openSSLRSAPKCS1Padding.InfluencingOn(openSSLRSARelevantMethods));
openSSLVulnerabilities.Add(openSSLVulnerableEVPCtx.InfluencingOn(openSSLEVPRelevantMethods));

// Aggregate results
result.Add(cryptoPPVulnerabilities);
result.Add(botanVulnerabilities);
result.Add(openSSLVulnerabilities);