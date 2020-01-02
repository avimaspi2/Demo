/**
	This query searches for bad key sizes in Botan's RSA functions
*/

/**** Constant data ****/
int RSA_MINIMUM_KEY_SIZE = 2048;
CxList rsaBadSizes = Find_Number_Out_Of_Range((long)RSA_MINIMUM_KEY_SIZE -1, Int64.MaxValue);

/**** Gathering vulnerabilities ****/
CxList rsaFunctions = All.NewCxList();
rsaFunctions.Add(Find_Not_In_Range("botan/rsa.h", "botan_privkey_create_rsa", 2, RSA_MINIMUM_KEY_SIZE, null));

CxList privateKeyConstructs = Find_ObjectCreations().FindByShortName("RSA_PrivateKey");
CxList privateKeyConstrtucParameters = rsaBadSizes.GetParameters(privateKeyConstructs);

/**** Returning sinks ****/
result.Add(rsaFunctions);
result.Add(privateKeyConstrtucParameters);