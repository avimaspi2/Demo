// Find usage of weak cryptographic functions by name signature


List<string> weakHashNames = new List<string> {"*md5*","*SHA1*","*SHA_1*","*SHA-1*",
		"*RC2*", "*RC4*", "*RC5*", "*RC6*", "*MD4*", "*_des_*", "des_*", "*desencrypt*"};


// Methods such as md5(), MD5(), SHA(), etc.
CxList methods = Find_Methods();
CxList weakMethods = methods.FindByShortNames(weakHashNames, false);

// Classes and types, such as:
//      CryptoPP::MD5 hash;
CxList weakTypes = All.FindByTypes(weakHashNames.ToArray(), false);

result = weakMethods + weakTypes;

// Add specific weak encryption computations for each library
result.Add(Find_Weak_Encryption_OpenSSL());
result.Add(Find_Weak_Encryption_LibTomCrypt());
result.Add(Find_Weak_Encryption_Botan());