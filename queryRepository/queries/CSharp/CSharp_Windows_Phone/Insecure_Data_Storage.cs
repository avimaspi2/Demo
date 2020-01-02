// Query - Insecure Data Storage
// ////////////////////////////-
// This risk addresses sensitive data left unprotected
// Code example:
// 			Password saved unencrypted
// How to Resolve:
// 			Encrypt password
//

// The purpose of the query is to detect any attempt to write unencrypted password


CxList passwords = Find_All_Passwords();
CxList writes = Find_Write();
CxList encrypt = Find_Encrypt();

result = writes.InfluencedByAndNotSanitized(passwords, encrypt);