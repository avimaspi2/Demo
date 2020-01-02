/**
	The following query will find all usages of salts without good randomization being used in 
		Botan implementation of PBKDF2 algorithm. For this implementation the good practice
		requires the usage of random_vec before the PBKDF2 method invocation. 
**/

// Find sanitizers, by finding all usages of arrays passed in openssl function "RAND_bytes"
CxList randombytesBufMethods = Find_Methods().FindByShortName("random_vec");

//Botan derive funcitons
CxList deriveKey = Find_Botan_PBDKF2_Function("derive_key");
CxList pbdkf2 = Find_Botan_PBDKF2_Function("pbdkf");
CxList pbdkfIterations = Find_Botan_PBDKF2_Function("pbdkf_iterations");
CxList pbdkfTimed = Find_Botan_PBDKF2_Function("pbdkf_timed");

//Case 1: function has 4 parameters
//derive_Key(out_len, passphrase, salt[], iterations)
CxList botanCase1 = Filter_By_Parameter_Count(deriveKey, 4);
CxList salts = All.GetParameters(botanCase1, 2);

//Case 2: function has 5 parameters
//derive_Key(out_len, passphrase, salt[], salt_len, iterations)
//derive_Key(out_len, passphrase, salt[], msec, iterations)
//pbdkf_iterations(out_len, passphrase, salt[], salt_len, iterations)
CxList botanCase2 = Filter_By_Parameter_Count(deriveKey + pbdkfIterations, 5);
salts += All.GetParameters(botanCase2, 2);

//Case 3: function has 6 parameters
//derive_Key(out_len, passphrase, salt[], salt_len, msec, iterations)
//pbdkf_timed(out_len, passphrase, salt[], salt_len, msec, iterations)
CxList botanCase3 = Filter_By_Parameter_Count(deriveKey + pbdkfTimed + pbdkfIterations, 6);
salts += All.GetParameters(botanCase3, 2);

//Case 4: function has 6 parameters
//pbdkf_iterations(out[],out_len, passphrase, salt[], salt_len, iterations)
CxList botanCase4 = Filter_By_Parameter_Count(pbdkfIterations, 6);
salts += All.GetParameters(botanCase4, 3);

//Case 4: function has 7 parameters
//pbdkf_timed(out[],out_len, passphrase, salt[], salt_len, msec, iterations)
//pbdkf(out[],out_len, passphrase, salt[], salt_len, iterations, msec)
CxList botanCase5 = Filter_By_Parameter_Count(pbdkfTimed + pbdkf2, 7);
salts += All.GetParameters(botanCase5, 3);
salts -= Find_Parameters();

CxList randomSalts = salts.DataInfluencedBy(randombytesBufMethods);

// Return the flow from variable declaration until the weak salt usage:
CxList predictableSalts = salts - randomSalts ;
CxList ObjectOfPredictableSalts = predictableSalts * Find_Unknown_References();
ObjectOfPredictableSalts .Add(predictableSalts * Find_Methods());
ObjectOfPredictableSalts.Add(All.FindByFathers(predictableSalts.FindByType(typeof(UnaryExpr))));
	

result = predictableSalts.DataInfluencedBy(All.FindDefinition(ObjectOfPredictableSalts));