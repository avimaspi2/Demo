/**
	The following query will find all usages of salts without good randomization being used in 
		limsodium implementation of Scrypt algorithm. For this implementation the good practice
		requires the usage of randombytes_buf before the crypto_pwhash_scryptsalsa208sha256_ll execution. 
*/

// Find all possible salt variables
CxList possibleSalts = All.NewCxList();
possibleSalts.Add(Find_Arrays());
possibleSalts.Add(Find_Unknown_References());


// Find sanitizers, by finding all usages of arrays passed in libsodium function "randombytes_buf"
CxList randombytesBugMethods = Find_Methods().FindByShortName("randombytes_buf");
CxList randombytesBugParameters = All.GetParameters(randombytesBugMethods,0);
CxList randombytes_buf_first_parameters_references = All.FindAllReferences(randombytesBugParameters);
CxList libSodiumSanitizers = randombytes_buf_first_parameters_references;


// Find all salts being used in libsodium's Scrypt calls
CxList libSodiumScryptExecutions = Find_Methods().FindByShortName("crypto_pwhash_scryptsalsa208sha256_ll");
CxList scryptSaltParameters = All.GetParameters(libSodiumScryptExecutions, 2);
CxList scryptSaltArrays = Find_Unknown_References().FindByFathers(scryptSaltParameters);


// Return the flow from variable declaration until the weak salt usage:
result = possibleSalts.InfluencingOnAndNotSanitized(scryptSaltArrays, libSodiumSanitizers);