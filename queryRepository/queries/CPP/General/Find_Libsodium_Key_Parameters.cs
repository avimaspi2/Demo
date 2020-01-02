CxList methods = Find_Methods();

List < string > libsodiumFunctions2 = new List<string> {
		"crypto_secretstream_xchacha20poly1305_init_push", 
		"crypto_secretstream_xchacha20poly1305_init_pull" };

List < string > libsodiumFunctions3 = new List<string> { 
		"crypto_stream_chacha20",
		"crypto_stream_chacha20_ietf",
		"crypto_stream_xchacha20",
		"crypto_stream_salsa20",
		"crypto_stream_salsa2012",
		"crypto_stream_salsa208",
		"crypto_stream" };

List < string > libsodiumFunctions4 = new List<string> { 
		"crypto_secretbox_easy",
		"crypto_secretbox_open_easy",
		"crypto_stream_chacha20_xor",
		"crypto_stream_chacha20_ietf_xor",
		"crypto_stream_xchacha20_xor",
		"crypto_stream_salsa20_xor",
		"crypto_stream_salsa2012_xor",
		"crypto_stream_salsa208_xor",
		"crypto_stream_xor" };

List < string > libsodiumFunctions5 = new List<string> { 
		"crypto_secretbox_detached",
		"crypto_secretbox_open_detached",
		"crypto_stream_chacha20_xor_ic",
		"crypto_stream_chacha20_ietf_xor_ic",
		"crypto_stream_xchacha20_xor_ic",
		"crypto_stream_salsa20_xor_ic" };

List<string> libsodiumFunctions8 = new List<string> {
		"crypto_aead_chacha20poly1305_encrypt",
		"crypto_aead_chacha20poly1305_decrypt",
		"crypto_aead_chacha20poly1305_decrypt_detached",
		"crypto_aead_chacha20poly1305_ietf_encrypt",
		"crypto_aead_chacha20poly1305_ietf_decrypt",
		"crypto_aead_chacha20poly1305_ietf_decrypt_detached",
		"crypto_aead_xchacha20poly1305_ietf_encrypt",
		"crypto_aead_xchacha20poly1305_ietf_decrypt",
		"crypto_aead_xchacha20poly1305_ietf_decrypt_detached",
		"crypto_aead_aes256gcm_encrypt",
		"crypto_aead_aes256gcm_decrypt",
		"crypto_aead_aes256gcm_decrypt_detached" };

List < string > libsodiumFunctions9 = new List<string> { 
		"crypto_aead_chacha20poly1305_encrypt_detached",
		"crypto_aead_chacha20poly1305_ietf_encrypt_detached",
		"crypto_aead_xchacha20poly1305_ietf_encrypt_detached",
		"crypto_aead_aes256gcm_encrypt_detached" };

CxList libsodiumMethods1 = methods.FindByShortName("crypto_aead_aes256gcm_beforenm");
CxList libsodiumMethods2 = methods.FindByShortNames(libsodiumFunctions2);
CxList libsodiumMethods3 = methods.FindByShortNames(libsodiumFunctions3);
CxList libsodiumMethods4 = methods.FindByShortNames(libsodiumFunctions4);
CxList libsodiumMethods5 = methods.FindByShortNames(libsodiumFunctions5);
CxList libsodiumMethods8 = methods.FindByShortNames(libsodiumFunctions8);
CxList libsodiumMethods9 = methods.FindByShortNames(libsodiumFunctions9);


result = All.GetParameters(libsodiumMethods1, 1);
result.Add(All.GetParameters(libsodiumMethods2, 2));
result.Add(All.GetParameters(libsodiumMethods3, 3));
result.Add(All.GetParameters(libsodiumMethods4, 4));
result.Add(All.GetParameters(libsodiumMethods5, 5));
result.Add(All.GetParameters(libsodiumMethods8, 8));
result.Add(All.GetParameters(libsodiumMethods9, 9));

result -= Find_Parameters();