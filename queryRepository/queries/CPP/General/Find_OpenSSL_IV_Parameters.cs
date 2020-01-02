//OpenSSL

CxList methods = Find_Methods();

// IV 5th Position
List<string> SetIVFunctions5thPosition = new List<string> {
		"DES_ncbc_encrypt", 
		"DES_pcbc_encrypt", 
		"DES_cfb64_encrypt", 
		"DES_ofb64_encrypt", 
		"DES_xcbc_encrypt", 
		"DES_cbc_cksum", 
		"DES_enc_write",
		"AES_cbc_encrypt", 
		"AES_cfb128_encrypt", 
		"AES_cfb1_encrypt", 
		"AES_cfb8_encrypt", 
		"AES_ofb128_encrypt", 
		"AES_ctr128_encrypt",
		"AES_ige_encrypt", 
		"BF_cbc_encrypt", 
		"BF_cfb64_encrypt", 
		"BF_ofb64_encrypt" };
		
// IV 6th Position
List<string> SetIVFunctions6thPosition = new List<string> {
		"DES_cfb_encrypt", 
		"DES_ofb_encrypt", 
		"DES_ede2_cbc_encrypt", 
		"DES_ede2_cfb64_encrypt", 
		"DES_ede2_ofb64_encrypt", 
		"AES_bi_ige_encrypt"};

// IV 7th Position 
List < string > SetIVFunctions7thPosition = new List<string> {
		"DES_ede3_cbc_encrypt", 
		"DES_ede3_cbcm_encrypt", 
		"DES_ede3_cfb64_encrypt", 
		"DES_ede3_ofb64_encrypt"};

//IV 8th Position
List < string > SetIVFunctions8thPosition = new List<string> {
		"DES_ede3_cbcm_encrypt"};

result = All.GetParameters(methods.FindByShortNames(SetIVFunctions5thPosition), 4);
result.Add(All.GetParameters(methods.FindByShortNames(SetIVFunctions6thPosition), 5));
result.Add(All.GetParameters(methods.FindByShortNames(SetIVFunctions7thPosition), 6));
result.Add(All.GetParameters(methods.FindByShortNames(SetIVFunctions8thPosition), 7));
result -= Find_Parameters();