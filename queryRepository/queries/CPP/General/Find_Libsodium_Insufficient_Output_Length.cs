/// <summary>
/// The following query will find insufficient output length.
/// The sufficient output length is variable through time, so this query receives that output length as parameter, that is required.
/// This query finds all output length that have a predictable size and that are being used in the libsodium's lib. 
/// </summary>

if(param.Length == 1)
{
	// Limit from where we consider it safe	
	int outputLength = (int) param[0];

	CxList methods = Find_Methods();
	CxList outputLengthNotInRange = All.NewCxList();
	
	// Output length in the second parameter		
	// ex: crypto_generichash(hash, sizeof hash, MESSAGE, MESSAGE_LEN, key, sizeof key);
	outputLengthNotInRange.Add(Find_Not_In_Range("sodium.h", "crypto_generichash", 1, outputLength, null));
	/* ex: crypto_pwhash (key, sizeof key, PASSWORD, strlen(PASSWORD), salt, 
	     				  	crypto_pwhash_OPSLIMIT_INTERACTIVE, crypto_pwhash_MEMLIMIT_INTERACTIVE, crypto_pwhash_ALG_DEFAULT)*/
	outputLengthNotInRange.Add(Find_Not_In_Range("sodium.h", "crypto_pwhash", 1, outputLength, null));
	/* ex: crypto_pwhash_scryptsalsa208sha256 (key, sizeof key, PASSWORD, 
						  	strlen(PASSWORD), salt, crypto_pwhash_scryptsalsa208sha256_OPSLIMIT_INTERACTIVE, 
    						crypto_pwhash_scryptsalsa208sha256_MEMLIMIT_INTERACTIVE*/
	outputLengthNotInRange.Add(Find_Not_In_Range("sodium.h", "crypto_pwhash_scryptsalsa208sha256", 1, outputLength, null));

	// Output length in the third parameter
	// ex: crypto_generichash_final(&state, hash, sizeof hash);
	outputLengthNotInRange.Add(Find_Not_In_Range("sodium.h", "crypto_generichash_final", 2, outputLength, null));
	
	// Output length in the fourth parameter
	// crypto_generichash_init(&state, key, sizeof key, sizeof hash);
	outputLengthNotInRange.Add(Find_Not_In_Range("sodium.h", "crypto_generichash_init", 3, outputLength, null));
	
	// Get know output length list (IntegerIntervalAbstractValue)
	CxList knownOutputLength = outputLengthNotInRange.FindByAbstractValue(x => x is IntegerIntervalAbstractValue);
		
	// Get unknown output length list (AnyAbstractValue and ObjectAbstractValue)
	CxList anyAbstractValue = outputLengthNotInRange.FindByAbstractValue(x => x is AnyAbstractValue);
	CxList objectAbstractValue = outputLengthNotInRange.FindByAbstractValue(x => x is ObjectAbstractValue);
	CxList unknownOutputLength = anyAbstractValue + objectAbstractValue;
	
	// Known constant with good size
	List<string> goodConstants = new List<string>{
		// generic hash constants
		"crypto_generichash_BYTES_MAX",						// size: 64U
		"crypto_generichash_BYTES",							// size: 32U
		"crypto_generichash_KEYBYTES_MAX",					// size: 64U
		"crypto_generichash_KEYBYTES",						// size: 32U
		
		// scrypt memory-hard function constants
		"crypto_pwhash_scryptsalsa208sha256_BYTES_MAX",		// size: 64UL
		"crypto_pwhash_scryptsalsa208sha256_PASSWD_MAX",	// size: 64ULL
		"crypto_pwhash_scryptsalsa208sha256_SALTBYTES", 	// size: 32U
		"crypto_pwhash_scryptsalsa208sha256_STRBYTES",		// size: 102U
		
		// argon2 memory-hard function constants
		"crypto_pwhash_BYTES_MAX",							// size: 64ULL
		"crypto_pwhash_STRBYTES"							// size: 128U
	};
	
	// Add bad and unknown hard coded constant to result
	// ex: 	crypto_generichash(hash, crypto_generichash_BYTES_MIN, MESSAGE, MESSAGE_LEN, key, sizeof key);
	anyAbstractValue = anyAbstractValue.FindByType(typeof (MemberAccess)) + anyAbstractValue.FindByType(typeof (UnknownReference));	
	CxList badOutputLength = anyAbstractValue - anyAbstractValue.FindByShortNames(goodConstants);
	CxList badOutputLengthDefinitions = All.FindDefinition(knownOutputLength);
	result.Add(badOutputLength);	
						
	// Get the known output length definitions
	CxList knownOutputLengthDefinitions = All.FindDefinition(knownOutputLength);

	// Add known output length 				
	result.Add(knownOutputLength.DataInfluencedBy(knownOutputLengthDefinitions));
						
	// Add hard coded output length values
	result.Add(knownOutputLength - result);
					
	// Find all output length with small size (based on the outputLength limit)
	IAbstractValue intervalZeroToOutputLength = new IntegerIntervalAbstractValue(0, outputLength - 1);
					
	// Handles the case where the output length is set with sizeof
	//ex: crypto_generichash(hash, sizeof hash, MESSAGE, MESSAGE_LEN, key, sizeof key)
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
	// hello.size()
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