/*
 * Returns the definitions of variables/constants that influence cryptographic keys 
 * (and the definitions of the cryptographic keys themselves)
 */

CxList methods = Find_Methods();

//get key based crypto methods
CxList cryptographic_methods = Find_Encryption_Methods();
CxList other_common_crypto_methods = methods.FindByShortName("SecKeyEncrypt:");
other_common_crypto_methods.Add(methods.FindByShortName("SecKeyDecrypt:"));
other_common_crypto_methods.Add(methods.FindByShortName("SecKeyRawSign:"));
other_common_crypto_methods.Add(methods.FindByShortName("SecKeyRawVerify:"));

//get the keys used in the crypto methods 
CxList crypto_keys = All.GetParameters(cryptographic_methods, 3);
crypto_keys.Add(All.GetParameters(other_common_crypto_methods, 0));

//find converters of strings into C strings and alike
//but get only those that are influenced by the crypto keys.
//(this is essential for cases when the keys are used in C 
//crypto methods as are the cases of the CCCrypt methods) 
CxList converters = Find_String_Converters();
converters = converters.InfluencedBy(All.FindDefinition(crypto_keys));

//also, obtain the target of these converters 
//(normally the key itself (in an NSString format or alike))
crypto_keys.Add(converters.GetTargetOfMembers());

//finally, obtain the definition of the elements that influence the crypto keys
CxList crypto_influences = All.FindDefinition(All.InfluencingOn(crypto_keys).FindByType(typeof(UnknownReference)));

result = crypto_influences;