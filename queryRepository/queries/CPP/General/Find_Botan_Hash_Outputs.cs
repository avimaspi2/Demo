string[] hashAlgorithms = {
	"Adler32", "Blake2b", "Comb4P", "CRC24", "CRC32", "GOST_34_11",
	"SHA_3*", "Keccak_1600", "Parallel", "SHAKE_*", "Skein_512", 
	"Streebog*", "MDx_HashFunction", "MD4", "MD5", "SHA_160",
	"SHA_512", "SHA_224", "SHA_256", "SHA_384", "SHA_512_256", "SM3", 
	"RIPEMD_160", "Tiger", "Whirpool", "HashFunction"  
	};

CxList potentialReferences = Find_Methods();
potentialReferences.Add(Find_ObjectCreations());
potentialReferences.Add(Find_UnknownReference());
potentialReferences.Add(Find_MemberAccesses());
	
CxList allReferences = potentialReferences.FindByTypes(hashAlgorithms, true);
CxList hashFunctions = allReferences.GetMembersOfTarget();
CxList final = hashFunctions.FindByShortName("final", true);

//Case 1: the hash is the return value 
List < string > methods = new List<string> {
		"create", "create_or_throw", "copy_state", "process", "final_stdvec"
		};
CxList case1 = hashFunctions.FindByShortNames(methods, true);
result.Add(All.GetParameters(case1, 0));
result.Add(Filter_By_Parameter_Count(final, 0));

//Case 2: the hash is the first parameter
CxList finalCase2 = Filter_By_Parameter_Count(final, 1);
result.Add(All.GetParameters(finalCase2, 0));