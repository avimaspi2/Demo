// Find all strings
CxList strings = Find_Strings();
//Find all (Char *)
strings.Add(Find_Char_Pointers());

//Find methods
CxList methods = Find_Methods();

//Find salt
CxList salt = All.FindByShortName("*salt*");
salt = All.InfluencedBy(salt);

////////////////////////////
///Cryptographic Librarys//
//////////////////////////

result.Add(Find_Hash_Without_Salt_OpenSSL(strings, methods, salt)); //openSSL
result.Add(Find_Hash_Without_Salt_CryptoPP(strings, methods, salt));//Crypto++
result.Add(Find_Hash_Without_Salt_UnixCrypt(strings, methods, salt));//crypt(unix)

result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);