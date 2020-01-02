/**
	The following query will find all usages of salts without a good size. 
	This query finds all the salts that have a predictable size being and that are being used in the Cryptopp PBKDF2.

**/

if(param.Length == 1)
{
	
	int saltSize = (int) param[0];

	CxList paramList = Find_Parameters();
		
	CxList cryptoppPBKDF2Executions = Find_Cryptopp_PBDKF2();
	CxList salts = All.GetParameters(cryptoppPBKDF2Executions, 6);

	//Starts salt size evaluation
	IAbstractValue intervalZeroToSaltSize = new IntegerIntervalAbstractValue(saltSize, Int32.MaxValue); 

		//Add know salt values to the small salt list.
	CxList knownSalts = salts.FindByAbstractValue(x => x is IntegerIntervalAbstractValue);
	knownSalts = knownSalts.FindByAbstractValue(abstractValue => !abstractValue.IncludedIn(intervalZeroToSaltSize, false));

		//Creates a unknownSalts List.
	CxList unknownSalts = salts.FindByAbstractValue(x => x is AnyAbstractValue || x is ObjectAbstractValue);
	
		//Creates a saltDefinitionList with all the salt definitions.
	CxList saltDefinitions = All.FindAllReferences(unknownSalts.GetTargetOfMembers()).FindByAssignmentSide(CxList.AssignmentSide.Left);
	saltDefinitions += All.FindAllReferences(unknownSalts).FindByAssignmentSide(CxList.AssignmentSide.Left);
	
	unknownSalts -= All.FindAllReferences(All.FindDefinition(unknownSalts.GetTargetsWithMembers())).GetMembersWithTargets();
	
	CxList saltDefinitionAssigners = saltDefinitions.GetAssigner();
	
		//Adds the saltDefinition 3rd parameter (lenght) to the parameters to evaluation;
		//ex. salt("TestString", 0, 30);
	CxList paramsToEvaluate = All.GetParameters(Filter_By_Parameter_Count(saltDefinitionAssigners, 3), 2);
		//Adds the saltDefinition 2nd parameter (lenght) to the parameters to evaluation;
		//ex. salt("TestString", 30);
	paramsToEvaluate.Add(All.GetParameters(Filter_By_Parameter_Count(saltDefinitionAssigners, 2), 1));
		//Adds the saltDefinition 1st parameter (lenght) to the parameters to evaluation;
		//ex. SecByteBlock pwsalt(AES::DEFAULT_KEYLENGTH);
	paramsToEvaluate.Add(All.GetParameters(Filter_By_Parameter_Count(saltDefinitionAssigners, 1), 0));
	
		//Removes duplicated values;
	paramsToEvaluate -= paramList;
	CxList paramsToEvaluateCastExprs = paramsToEvaluate.FindByType(typeof(CastExpr));
	paramsToEvaluate.Add(All.FindByFathers(paramsToEvaluateCastExprs) - Find_TypeRef());
	paramsToEvaluate -= paramsToEvaluateCastExprs;

		//Adds all salts with a lenght < saltSize;
	CxList smallSalts = paramsToEvaluate.FindByAbstractValue(abstractValue => !abstractValue.IncludedIn(intervalZeroToSaltSize, false));
		
	CxList knownSaltsDefinitions = All.FindDefinition(knownSalts);

	result.Add(smallSalts.DataInfluencingOn(unknownSalts));
		
		//Adds hard coded salt values
	result.Add(knownSalts - result);

		//Adds unknown salts that cannot be evaluated
	result.Add(unknownSalts - All.FindAllReferences(All.FindDefinition(unknownSalts)) - All.FindAllReferences(All.FindDefinition(unknownSalts.GetTargetOfMembers())).GetMembersOfTarget());

}