/**
 	The following query will find: 
        - all usages of salts without good randomization being used in Scrypt encryptions.
 		- all usages of salts arrays with a size smaller than 32.
	This query aggregates the different related queries by c++ library.
*/

long saltSize = 32;

// Scrypt_Weak_Salt_Value in LibSodium
CxList libSodiumResults = All.NewCxList();
libSodiumResults.Add(Find_Libsodium_Scrypt_Weak_Salt_Randomization());
libSodiumResults.Add(Find_Libsodium_Scrypt_Weak_Salt_Size(saltSize));

// Results
result = libSodiumResults.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);