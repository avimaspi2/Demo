/// <summary>
/// The following query will find Insecure Symmetric Encryption Cipher Mode in the Open SSL library
/// </summary>

// get all the insecure cipher methods that perform encryption and / or decryption
CxList insecureCipherModes = Find_Methods().FindByShortNames(new List<string>{
	/* Electronic Codebook Mode (ECB) */
	"DES_ecb_encrypt",			// DES
	"DES_ecb2_encrypt",			// DES
	"DES_ecb3_encrypt",			// DES
	"BF_ecb_encrypt",			// Blowfish
	"AES_ecb_encrypt",			// AES (Rijndael)
			
	// Output Feedback Mode (OFB)
	"DES_ofb_encrypt",			// DES
	"DES_ofb_encrypt",			// DES
	"DES_ofb64_encrypt",		// DES
	"DES_ede3_ofb64_encrypt",	// DES
	"DES_ede2_ofb64_encrypt",	// DES
	"DES_ofb64_encrypt",		// DES
	"DES_ede3_ofb64_encrypt",	// DES	
	"DES_ede2_ofb64_encrypt",	// DES
	"BF_ofb64_encrypt",			// Blowfish
	"AES_ofb128_encrypt",		// AES (Rijndael)

	// Cipher Feedback Mode (CFB)
	"DES_cfb_encrypt",			// DES
	"DES_cfb64_encrypt",		// DES
	"DES_ede3_cfb64_encrypt",	// DES
	"DES_ede2_cfb64_encrypt",	// DES
	"BF_cfb64_encrypt",			// Blowfish
	"AES_cfb128_encrypt",		// AES (Rijndael)
	"AES_cfb1_encrypt",			// AES (Rijndael)
	"AES_cfb8_encrypt",			// AES (Rijndael)
			
	// Cipher Block Chaining (CBC)
	"DES_ncbc_encrypt",			// DES
	"DES_xcbc_encrypt",			// DES
	"DES_ede3_cbc_encrypt",		// DES
	"DES_ede2_cbc_encrypt",		// DES
	"DES_pcbc_encrypt",			// DES
	"BF_cbc_encrypt",			// Blowfish
	"AES_cbc_encrypt",			// AES (Rijndael)

	// Counter Mode (CTR)
	"AES_ctr128_encrypt"		// AES (Rijndael)	
});

// get insecure ecryption cipher algorithms
CxList insecureEncryptionAlgorithm = Find_Methods().FindByShortNames(new List<string>{
	/* Electronic Codebook Mode (ECB) */
	"EVP_des_ecb",				// DES
	"EVP_bf_ecb",				// Blowfish
	"EVP_aes_128_ecb",			// AES (Rijndael)
	"EVP_aes_192_ecb",			// AES (Rijndael)
	"EVP_aes_256_ecb",			// AES (Rijndael)
		
	// Output Feedback Mode (OFB)
	"EVP_des_ofb",				// DES
	"EVP_des_ede_ofb",			// DES
	"EVP_des_ede3_ofb",			// DES
	"EVP_bf_ofb",				// Blowfish
	"EVP_aes_128_ofb",			// AES (Rijndael)
	"EVP_aes_192_ofb",			// AES (Rijndael)
	"EVP_aes_256_ofb",			// AES (Rijndael)
		
	// Cipher Feedback Mode (CFB)
	"EVP_des_cfb",				// DES
	"EVP_des_ede_cfb",			// DES
	"EVP_des_ede3_cfb",			// DES
	"EVP_bf_cfb",				// Blowfish
	"EVP_aes_128_cfb",			// AES (Rijndael)
	"EVP_aes_192_cfb",			// AES (Rijndael)
	"EVP_aes_256_cfb",			// AES (Rijndael)
		
	// Cipher Block Chaining (CBC)
	"EVP_des_cbc",				// DES
	"EVP_des_ede_cbc",			// DES
	"EVP_des_ede3_cbc",			// DES
	"EVP_bf_cbc",				// Blowfish
	"EVP_aes_128_cbc",			// AES (Rijndael)
	"EVP_aes_192_cbc",			// AES (Rijndael)
	"EVP_aes_256_cbc",			// AES (Rijndael)
}); 

// get all the cipher modes used as parameter to setup a cipher context (ctx) for encryption
CxList insecureCipherModesParams = All.GetParameters(Find_Methods().FindByShortName("EVP_EncryptInit_ex"), 1) - Find_Param();

// get all the insecure cipher modes used for encryption
insecureCipherModes.Add(insecureEncryptionAlgorithm.DataInfluencingOn(insecureCipherModesParams));
insecureCipherModes.Add(insecureCipherModesParams * insecureEncryptionAlgorithm);
result = insecureCipherModes;