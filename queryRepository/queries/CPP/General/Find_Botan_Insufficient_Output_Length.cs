/// <summary>
/// The following query will find insufficient output length.
/// This sufficient output length is variable through time, so this library receives that output length as parameter, that is required.
/// This query finds all output length that have a predictable size and that are being used in the Botan's lib. 
/// </summary>

if(param.Length == 1)
{
	// Limit from where we consider it safe	
	int outputLength = (int) param[0];

	CxList methods = Find_Methods();
	CxList paramList = Find_Parameters();
	CxList outputLengthNotInRange = All.NewCxList();

	// Get the interval abstrac value (based on the outputLength limit)
	IAbstractValue intervalZeroToOutputLength = new IntegerIntervalAbstractValue(0, outputLength - 1);
	
	//Botan derive funcitons
	CxList deriveKey = Find_Botan_PBDKF2_Function("derive_key");	

	// RandomNumberGenerator
	// ex: pbkdf->derive_key(40, "password", &salt[0],  salt.size(), 30, getIteration())
	outputLengthNotInRange.Add(Find_Not_In_Range(deriveKey, 0, outputLength, null));
		
	// Get know output length list (IntegerIntervalAbstractValue)
	CxList knownOutputLength = outputLengthNotInRange.FindByAbstractValue(x => x is IntegerIntervalAbstractValue);
					
	// Get a unknown output length list (AnyAbstractValue and ObjectAbstractValue)
	CxList anyAbstractValue = outputLengthNotInRange.FindByAbstractValue(x => x is AnyAbstractValue);
	CxList objectAbstractValue = outputLengthNotInRange.FindByAbstractValue(x => x is ObjectAbstractValue);
	CxList unknownOutputLength = anyAbstractValue + objectAbstractValue;
				
	// Get the unknown output length definitions
	CxList outputLengthDefinitions = All.FindAllReferences(unknownOutputLength.GetTargetOfMembers()).FindByAssignmentSide(CxList.AssignmentSide.Left);
	outputLengthDefinitions.Add(All.FindAllReferences(unknownOutputLength).FindByAssignmentSide(CxList.AssignmentSide.Left));
		
	// Get the known output length definitions
	CxList knownOutputLengthDefinitions = All.FindDefinition(knownOutputLength);
	
	// Add known output length 				
	result.Add(knownOutputLength.DataInfluencedBy(knownOutputLengthDefinitions));
			
	// Add hard coded output length values
	// ex: 
	result.Add(knownOutputLength - result);

	// Handles the case where the output length is set with sizeof
	//ex: 
	CxList outputLengthNotInRangeParameters = Find_Unknown_References().GetParameters(outputLengthNotInRange);
	CxList sSize = outputLengthNotInRangeParameters.GetParameters(methods.FindByShortName("sizeof"), 0);
	
	if(sSize.Count > 0)
	{	
		CxList sizeOfAbtractValue = sSize.FindByAbstractValue(abstractValue => abstractValue.IncludedIn(intervalZeroToOutputLength, false));

		if(sizeOfAbtractValue.Count > 0)
		{	
			CxList sizeOfDefinitions = All.FindDefinition(sizeOfAbtractValue);
			result.Add(sizeOfAbtractValue.DataInfluencedBy(sizeOfDefinitions));
		}
	}

	// Add unknown output length with target members
	// std::string hello = "Hello";
	// ex: 
	outputLengthNotInRangeParameters.Add(unknownOutputLength.GetTargetOfMembers());

	// Get the output length list (ObjectAbstractValue and StringAbstractValue)
	CxList smallOutputLength = outputLengthNotInRangeParameters.FindByAbstractValue(abstractValue => {                          
									
		int count = 0;
		ObjectAbstractValue objAbstractValue = (abstractValue as ObjectAbstractValue);
		StringAbstractValue strAbstractValue = (abstractValue as StringAbstractValue);
		
		if( objAbstractValue != null ) 
		{
			if( objAbstractValue.AllocatedSize != null )
			{
				// get lower interval value
				// ex: char hash[2];
				count = (int) objAbstractValue.AllocatedSize.LowerIntervalBound;
			}
			else
			{
				// count number of fields
				// ex: char example[5] = {'H', 'e', 'l', 'l', 'o'};
				foreach( string key in objAbstractValue.FieldNames ) count++;
			}
		}
		else if(strAbstractValue != null) 
		{
			// string size
			// ex: std::string string = "Hello";
			count = (int) strAbstractValue.Content.Length;
		}
	
		IAbstractValue currentOutputLength = new IntegerIntervalAbstractValue(count, count); 		
		return currentOutputLength.IncludedIn(intervalZeroToOutputLength, false);
	});
			
	smallOutputLength.Add(outputLengthNotInRangeParameters.FindByAbstractValue(abstractValue => abstractValue.IncludedIn(intervalZeroToOutputLength, false)));
	CxList smallOutputLengthDefinitions = All.FindDefinition(smallOutputLength);
	result.Add(smallOutputLength.DataInfluencedBy(smallOutputLengthDefinitions));
}