/*

List of Safe Randoms.

*/

CxList methods = Find_Methods();

//Cryptopp
string[] cryptoppSafeRandoms = new string[] {"AutoSeededX917RNG", "RDRAND", "RDSEED", "RandomPool", "AutoSeededRandomPool"};
CxList cryptoppSafeGenerators = methods.FindByShortName("GenerateBlock").GetTargetOfMembers().FindByTypes(cryptoppSafeRandoms);
result = All.GetParameters(cryptoppSafeGenerators.GetMembersOfTarget(), 0) - Find_Parameters();
result.Add(methods.FindByShortName("GenerateByte").GetTargetOfMembers().FindByTypes(cryptoppSafeRandoms).GetMembersOfTarget());

//Botan
string[] botanSafeArray = new string[] {"AutoSeeded_RNG", "HMAC_DRBG", "RDRAND_RNG", "System_RNG"};

CxList botanSafeGenerators = All.FindByParameters(Find_ObjectCreations().FindByShortNames(new List<string> (botanSafeArray))).GetAssignee();
CxList rngValue = All.FindAllReferences(botanSafeGenerators).GetMembersOfTarget().FindByShortName("randomize");
result.Add((All.GetParameters(rngValue, 0) - Find_Parameters()).GetTargetOfMembers());
result.Add(methods.FindByShortNames(new List<string> { "rdrand", "next_byte" }));

//OpenSSL
List<string> openSSLSafeRandoms = new List<string> {"RAND_Bytes", "RAND_priv_bytes"};
CxList openSSLsafeGenerators = All.GetParameters(methods.FindByShortName("RAND_bytes"), 0) - Find_Parameters();
result.Add(openSSLsafeGenerators);

//Libosidum
result.Add(methods.FindByShortNames(new List<string> { "randombytes_uniform", "randombytes_random" }));