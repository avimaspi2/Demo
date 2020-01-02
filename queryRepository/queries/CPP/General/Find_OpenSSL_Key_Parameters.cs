CxList methods = Find_Methods();

List<string> setKeysFunctions =
	new List<string> {"DES_set_key", "DES_set_key_checked", "DES_set_key_unchecked", "DES_string_to_key",
		"DES_string_to_2keys", "AES_set_decrypt_key", "AES_set_encrypt_key"};
		
CxList setKeysMethods = methods.FindByShortNames(setKeysFunctions);
CxList keyParams = All.GetParameters(setKeysMethods, 0);

setKeysFunctions = new List<string> {"BF_set_key", "RC4_set_key"};
setKeysMethods = methods.FindByShortNames(setKeysFunctions);
keyParams.Add(All.GetParameters(setKeysMethods, 2));

keyParams -= Find_Parameters();


// Usually is made a cast of the 1st parameter and we only want the unknown reference
// DES_set_key((C_Block *)key1, &ks1);
CxList castExpr = keyParams.FindByType(typeof(CastExpr));
keyParams -= castExpr;
CxList key_refs_params = Find_Expressions().FindByFathers(castExpr);

result = keyParams;
result.Add(key_refs_params);