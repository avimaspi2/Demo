List < string > keyGenFunctions = new List<string> {
		"crypto_aead_chacha20poly1305_keygen", 
		"crypto_aead_chacha20poly1305_ietf_keygen",
		"crypto_aead_xchacha20poly1305_ietf_keygen",
		"crypto_aead_aes256gcm_keygen"};

CxList methods = Find_Methods().FindByShortNames(keyGenFunctions);
result = All.GetParameters(methods, 0) - Find_Parameters();