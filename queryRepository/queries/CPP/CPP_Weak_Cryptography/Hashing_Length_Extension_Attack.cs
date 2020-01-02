/*
Query Logic
This query should find uses of hashes in a structure that creates a signature/MAC
by concatenating a secret with other data, which can be done in several different ways: 

let signature = hash(secret || data) = 6036708eba0d11f6ef52ad44e8b74d5b
h = Hash(); h.update(secret); h.update(data); h.digest()
h = Hash(); h.update(secret); h.digest(data)

Inputs
MD4,MD5,RIPEMD-160,SHA-0,SHA-1
SHA-256,SHA-512,WHIRLPOOL
*/
List<string> vulnHashNames = new List<string> {"*md4*","*md5*","*RIPEMD-160*","*SHA0*","*SHA_0*","*SHA-0*",
		"*SHA1*","*SHA_1*","*SHA-1*","*SHA256*","*SHA_256*","*SHA-256*","*SHA512*","*SHA_512*","*SHA-512*","*WHIRLPOOL*"};


// Find Methods:
CxList methods = Find_Methods();
CxList vulnHashes = methods.FindByShortNames(vulnHashNames, false);

CxList decls = Find_Declarators();
decls.Add(Find_UnknownReference());
decls.Add(Find_MemberAccesses());

// Classes and types, such as:
//      CryptoPP::MD5 hash;
vulnHashes.Add(decls.FindByTypes(vulnHashNames.ToArray(), false));
//Remove false positives and/or duplicate results of the type "byte digest[ MD5::DIGESTSIZE ];"
vulnHashes -= vulnHashes.GetMembersOfTarget().FindByShortName("Digestsize", false).GetTargetOfMembers();

/*
Sinks
The last digest() method call that generates the final hash.
Find digest
*/
CxList digest = methods.FindByShortName("*digest", false);

/*
Sanitizers
Use of a hash function not vulnerable to length extension (sodium_generichash(), which uses Blake2b) 
Use of HMAC construction (becomes sanitized even if it's HMAC-MD5, HMAC-SHA0, etc.)
*/
List<string> sanitizerNames = new List<string> {"HMAC*","crypto_generichash","Blake2b"};
CxList sanitizers = methods.FindByShortNames(sanitizerNames, false);
sanitizers.Add(decls.FindByTypes(sanitizerNames.ToArray(), false));

result = digest.InfluencedByAndNotSanitized(vulnHashes, sanitizers);