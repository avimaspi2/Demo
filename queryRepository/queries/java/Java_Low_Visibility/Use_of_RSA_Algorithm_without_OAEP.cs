// Use_Of_RSA_Algorithm_without_OAEP
 
// This query finds cipher RSA cryptographic 
// Algorithm without OAEP padding

CxList cipherInstance = All.FindByMemberAccess("Cipher.getInstance"); 

CxList stringLiterals = Find_Strings();
CxList cipherRSA = stringLiterals.FindByShortName("RSA*");
CxList paddingOAEP = stringLiterals.FindByShortName("*OAEP*");

CxList notSecured = cipherRSA - paddingOAEP;

result = cipherInstance.DataInfluencedBy(notSecured);