// Query Broken Cryptography
// Finds cryptography keys hard coded or saved unencrypted
//////////////////////////////////////////////////////////

CxList strings = Find_Strings();

CxList keys = All.FindByMemberAccess("Rijndael*.Key");
keys.Add(All.FindByMemberAccess("Aes*.Key"));
keys.Add(All.FindByMemberAccess("DES*.Key"));
keys.Add(All.FindByMemberAccess("HMAC*.Key"));
keys.Add(All.FindByMemberAccess("TripleDES*.Key"));
keys.Add(All.FindByMemberAccess("ECDiffieHellman*.Key"));
keys.Add(All.FindByMemberAccess("AsymmetricKey*.SetKey"));
keys.Add(All.FindByMemberAccess("AsymmetricSignature*.SetKey"));

CxList dpapiEnc = All.FindByMemberAccess("ProtectedData.Protect") +
				  All.FindByMemberAccess("ProtectedData.Unprotect");	
CxList dpapiKey =  All.GetParameters(dpapiEnc, 1);

keys.Add(dpapiKey);

CxList hardCoded = All.FindByType(typeof(StringLiteral)) +
				   All.FindByType(typeof(IntegerLiteral));

result = hardCoded.DataInfluencingOn(keys);