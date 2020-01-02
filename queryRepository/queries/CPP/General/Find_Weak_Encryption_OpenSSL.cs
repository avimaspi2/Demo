/**
 Find weak encryption from OpenSSL
 **/

CxList methods = Find_Methods();
CxList parameters = Find_Parameters();

// OpenSSL - MD5
CxList MD5 = Find_Members_By_Include("openssl/md5.h", new string[]{"MD5"});
CxList MD5_Outputs = All.GetParameters(MD5, 2) - parameters;

// OpenSSL - SHA-1 
CxList SHA1 = Find_Members_By_Include("openssl/sha.h", new string[]{"SHA1"});
CxList SHA1_Outputs = All.GetParameters(SHA1, 2) - parameters;

// OpenSSL - RC2
CxList RC2 = Find_Members_By_Include("openssl/rc2.h", new string[]{"RC2_encrypt"});
CxList RC2_Outputs = All.GetParameters(RC2, 0) - parameters;


result.Add(SHA1.ConcatenatePath(SHA1_Outputs, false));
result.Add(SHA1);

result.Add(MD5.ConcatenatePath(MD5_Outputs, false));
result.Add(MD5);

result.Add(RC2.ConcatenatePath(RC2_Outputs, false));
result.Add(RC2);