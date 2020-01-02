/**
	The following query will find all usages of salts without good randomization being used in 
		openSSL implementation of PBKDF2 algorithm. For this implementation the good practice
		requires the usage of randombytes_buf before the PKCS5_PBKDF2_HMAC_SHA1 execution. 
**/

// Find all possible salt variables
CxList possibleSalts = Find_Arrays();
possibleSalts.Add(Find_Unknown_References().FindByAssignmentSide(CxList.AssignmentSide.Left));
possibleSalts.Add(All.FindByType(typeof(Declarator)));

// Find sanitizers, by finding all usages of arrays passed in openssl function "RAND_bytes"
CxList randombytesBufMethods = Find_Methods().FindByShortName("RAND_bytes");
CxList randombytesBufParameters = All.GetParameters(randombytesBufMethods, 0);
CxList randombytes_buf_first_parameters_references = All.FindAllReferences(randombytesBufParameters);
CxList openSSLSanitizers = randombytes_buf_first_parameters_references;

// Find all salts being used in openSSL's Scrypt calls
CxList libSodiumScryptExecutions = Find_Methods().FindByShortName("PKCS5_PBKDF2_HMAC*");
CxList scryptSaltParameters = All.GetParameters(libSodiumScryptExecutions, 2);
CxList scryptSaltArrays = Find_Unknown_References().FindByFathers(scryptSaltParameters);

// Return the flow from variable declaration until the weak salt usage:
result = possibleSalts.InfluencingOnAndNotSanitized(scryptSaltArrays, openSSLSanitizers);