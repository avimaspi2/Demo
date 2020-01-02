//////////////////////////////////////////////////////////////
// Query Weak_Encryption
// DRD17-J Do not use the Android cryptographic security 
// provider encryption default for AES
// The query looks for use of AES with ECB block encryption
//////////////////////////////////////////////////////////////

CxList methods = Find_Methods();
CxList strings = Find_Strings();

CxList cipherGetInstance = methods.FindByMemberAccess("Cipher.getInstance");
CxList encryptionStrings = strings.GetParameters(cipherGetInstance, 0);

foreach(CxList encStr in encryptionStrings)
{
	String str = encStr.GetName();
	if(str.Equals("AES") || str.StartsWith(@"AES/ECB"))
	{
		result.Add(encStr);
	}
}