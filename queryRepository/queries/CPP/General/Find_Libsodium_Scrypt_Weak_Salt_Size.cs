/**
	The following query will find all usages of salts without a good size. This size is variable through time,
		so this library receives that size as parameter, that is required.

	This query finds all the salts that have a predictable size being and that are being used in the libsodium's 
		Scrypt implementation

*/

if(param.Length == 1){
	
	long saltSize = (long) param[0];

	// Find all salts being used in libsodium's Scrypt calls
	CxList libSodiumScryptExecutions = Find_Methods().FindByShortName("crypto_pwhash_scryptsalsa208sha256_ll");
	CxList scryptSaltParameters = All.GetParameters(libSodiumScryptExecutions, 2);
	CxList scryptSaltArrays = Find_Unknown_References().FindByFathers(scryptSaltParameters);

	// Find all salts with small size:
	CxList smallSalts = Find_Arrays_By_Size(long.MinValue, saltSize );

	// Return the flow from variable declaration until the weak salt usage:
	result = smallSalts.DataInfluencingOn(scryptSaltArrays);
}