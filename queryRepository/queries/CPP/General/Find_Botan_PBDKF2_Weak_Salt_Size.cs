/**
	The following query will find all usages of salts without a good size. 
	This query finds all the salts that have a predictable size being and that are being used in the Botan PBKDF2.

*/

if(param.Length == 1)
{

	int saltSize = (int) param[0];

	CxList paramList = Find_Parameters();
	
	//Botan derive funcitons
	CxList deriveKey = Find_Botan_PBDKF2_Function("derive_key");
	CxList pbdkf2 = Find_Botan_PBDKF2_Function("pbdkf");
	CxList pbdkfIterations = Find_Botan_PBDKF2_Function("pbdkf_iterations");
	CxList pbdkfTimed = Find_Botan_PBDKF2_Function("pbdkf_timed");

	//Case 1: function has 4 parameters
	//derive_Key(out_len, passphrase, salt[], iterations)
	CxList botanCase1 = Filter_By_Parameter_Count(deriveKey, 4);
	CxList salts = Find_Not_In_Range(botanCase1, 2, saltSize, null);
	
	//Case2: function has 5 parameters
	//derive_Key(out_len, passphrase, salt[], salt_len, iterations)
	//derive_Key(out_len, passphrase, salt[], msec, iterations)
	//pbdkf_iterations(out_len, passphrase, salt[], salt_len, iterations)
	CxList botanCase2 = Filter_By_Parameter_Count(deriveKey + pbdkfIterations, 5);
	salts.Add(Find_Not_In_Range(botanCase2, 3, saltSize, null));
	
	//Case 3: function has 6 parameters
	//derive_Key(out_len, passphrase, salt[], salt_len, msec, iterations)
	//pbdkf_timed(out_len, passphrase, salt[], salt_len, msec, iterations)
	CxList botanCase3 = Filter_By_Parameter_Count(deriveKey + pbdkfTimed, 6);
	salts.Add(Find_Not_In_Range(botanCase3, 3, saltSize, null));

	//Case 4: function has 6 parameters
	//pbdkf_iterations(out[],out_len, passphrase, salt[], salt_len, iterations)
	CxList botanCase4 = Filter_By_Parameter_Count(pbdkfIterations, 6);
	salts.Add(Find_Not_In_Range(botanCase4, 4, saltSize, null));
	
	//Case 5: function has 7 parameters
	//pbdkf_timed(out[],out_len, passphrase, salt[], salt_len, msec, iterations)
	//pbdkf(out[],out_len, passphrase, salt[], salt_len, iterations, msec)
	CxList botanCase5 = Filter_By_Parameter_Count(pbdkfTimed + pbdkf2, 7);
	salts.Add(Find_Not_In_Range(botanCase5, 4, saltSize, null));

	//Add know salt values to the small salt list.
	CxList knownSalts = salts.FindByAbstractValue(x => x is IntegerIntervalAbstractValue);
	CxList smallSalts = knownSalts;
	
	//Creates a unknownSalts List.
	CxList unknownSalts = salts.FindByAbstractValue(x => x is AnyAbstractValue || x is ObjectAbstractValue);

	//Creates a saltDefinitionList with all the salt definitions.
	CxList saltDefinitions = All.FindAllReferences(unknownSalts.GetTargetOfMembers()).FindByAssignmentSide(CxList.AssignmentSide.Left);
	saltDefinitions.Add(All.FindAllReferences(unknownSalts).FindByAssignmentSide(CxList.AssignmentSide.Left));

	//Adds the random_vec value (lenght) to the parameters to evaluation;
	//ex. salt = rng.random_vec(16);
	CxList paramsToEvaluate = All.GetParameters(saltDefinitions.GetAssigner().FindByShortName("random_vec"));
	//Adds the saltDefinition 3rd parameter (lenght) to the parameters to evaluation;
	//ex. salt("TestString", 0, 30);
	paramsToEvaluate.Add(All.GetParameters(Filter_By_Parameter_Count(saltDefinitions.GetAssigner(), 3), 2));
	//Adds the saltDefinition 2nd parameter (lenght) to the parameters to evaluation;
	//ex. salt("TestString", 30);
	paramsToEvaluate.Add(All.GetParameters(Filter_By_Parameter_Count(saltDefinitions.GetAssigner(), 2), 1));
	//Removes duplicated values;
	paramsToEvaluate -= paramList;

	//Starts salt size evaluation
	IAbstractValue intervalZeroToSaltSize = new IntegerIntervalAbstractValue(0, saltSize - 1); 
	
	//Adds all salts with a lenght < saltSize;
	smallSalts.Add(paramsToEvaluate.FindByAbstractValue(abstractValue => abstractValue.IncludedIn(intervalZeroToSaltSize, false)));

	CxList knownSaltsDefinitions = All.FindDefinition(knownSalts);
	
	result.Add(smallSalts.DataInfluencingOn(unknownSalts));
	result.Add(knownSalts.DataInfluencedBy(knownSaltsDefinitions));
	
	//Adds hard coded salt values
	result.Add(knownSalts - result);
	
	//Adds unknown salts that cannot be evaluated
	result.Add(unknownSalts - All.FindAllReferences(saltDefinitions) - All.FindAllReferences(saltDefinitions).GetMembersOfTarget());
}