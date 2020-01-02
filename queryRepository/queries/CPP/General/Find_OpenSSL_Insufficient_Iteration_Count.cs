// According to the description, we assume 10.000 as the minimum secure threshold
// so we should yield a result for a value below 10.000
result = Find_Not_In_Range("openssl/evp.h", "PKCS5_PBKDF2_HMAC_SHA1", 4, 10000, null);
result.Add(Find_Not_In_Range("openssl/evp.h", "PKCS5_PBKDF2_HMAC", 4, 10000, null));