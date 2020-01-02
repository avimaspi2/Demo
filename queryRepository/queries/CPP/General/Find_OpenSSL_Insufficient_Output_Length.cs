/// <summary>
/// The following query will find insufficient output length.
/// The sufficient output length is variable through time, so this query receives that output length as parameter, that is required.
/// This query finds all output length that have a predictable size and that are being used in the OpenSSL's lib. 
/// </summary>

if(param.Length == 1)
{
	// Limit from where we consider it safe	
	int outputLength = (int) param[0];

	CxList methods = Find_Methods();
	CxList paramList = Find_Parameters();
	CxList outputLengthNotInRange = All.NewCxList();

	// Find all output length with small size (based on the outputLength limit)
	IAbstractValue intervalZeroToOutputLength = new IntegerIntervalAbstractValue(0, outputLength - 1);
	
	// TripleDES		
	outputLengthNotInRange.Add(Find_Not_In_Range("openssl/des.h", "DES_ncbc_encrypt", 2, outputLength, null));
	outputLengthNotInRange.Add(Find_Not_In_Range("openssl/des.h", "DES_cfb_encrypt", 3, outputLength, null));
	outputLengthNotInRange.Add(Find_Not_In_Range("openssl/des.h", "DES_pcbc_encrypt", 2, outputLength, null));	
	outputLengthNotInRange.Add(Find_Not_In_Range("openssl/des.h", "DES_cfb64_encrypt", 2, outputLength, null));
	outputLengthNotInRange.Add(Find_Not_In_Range("openssl/des.h", "DES_ofb64_encrypt", 2, outputLength, null));
	outputLengthNotInRange.Add(Find_Not_In_Range("openssl/des.h", "DES_xcbc_encrypt", 2, outputLength, null));
	outputLengthNotInRange.Add(Find_Not_In_Range("openssl/des.h", "DES_ede2_cbc_encrypt", 2, outputLength, null));
	outputLengthNotInRange.Add(Find_Not_In_Range("openssl/des.h", "DES_ede2_cfb64_encrypt", 2, outputLength, null));
	outputLengthNotInRange.Add(Find_Not_In_Range("openssl/des.h", "DES_ede2_ofb64_encrypt", 2, outputLength, null));
	outputLengthNotInRange.Add(Find_Not_In_Range("openssl/des.h", "DES_ede3_cbc_encrypt", 2, outputLength, null));
	outputLengthNotInRange.Add(Find_Not_In_Range("openssl/des.h", "DES_ede3_cfb64_encrypt", 2, outputLength, null));
	outputLengthNotInRange.Add(Find_Not_In_Range("openssl/des.h", "DES_ede3_ofb64_encrypt", 2, outputLength, null));
	outputLengthNotInRange.Add(Find_Not_In_Range("openssl/des.h", "DES_cbc_cksum", 2, outputLength, null));
	outputLengthNotInRange.Add(Find_Not_In_Range("openssl/des.h", "DES_quad_cksum", 2, outputLength, null));
	
	// Blowfish
	outputLengthNotInRange.Add(Find_Not_In_Range("openssl/blowfish.h", "BF_cbc_encrypt", 2, outputLength, null));
	outputLengthNotInRange.Add(Find_Not_In_Range("openssl/blowfish.h", "BF_cfb64_encrypt", 2, outputLength, null));
	outputLengthNotInRange.Add(Find_Not_In_Range("openssl/blowfish.h", "BF_ofb64_encrypt", 2, outputLength, null));
		
	// AES (Rijndael)
	outputLengthNotInRange.Add(Find_Not_In_Range("openssl/aes.h", "AES_cbc_encrypt", 2, outputLength, null));
	outputLengthNotInRange.Add(Find_Not_In_Range("openssl/aes.h", "AES_cfb128_encrypt", 2, outputLength, null));
	outputLengthNotInRange.Add(Find_Not_In_Range("openssl/aes.h", "AES_cfb1_encrypt", 2, outputLength, null));
	outputLengthNotInRange.Add(Find_Not_In_Range("openssl/aes.h", "AES_cfb8_encrypt", 2, outputLength, null));
	outputLengthNotInRange.Add(Find_Not_In_Range("openssl/aes.h", "AES_ofb128_encrypt", 2, outputLength, null));
	outputLengthNotInRange.Add(Find_Not_In_Range("openssl/aes.h", "AES_ctr128_encrypt", 2, outputLength, null));
	outputLengthNotInRange.Add(Find_Not_In_Range("openssl/aes.h", "AES_ige_encrypt", 2, outputLength, null));
	outputLengthNotInRange.Add(Find_Not_In_Range("openssl/aes.h", "AES_bi_ige_encrypt", 2, outputLength, null));
		
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
	// ex: DES_ncbc_encrypt(in, out, 20, &keysched, &ivec, DES_ENCRYPT);
	result.Add(knownOutputLength - result);

	// Handles the case where the output length is set with sizeof
	// ex: DES_ncbc_encrypt(in, out, sizeof(out), &keysched, &ivec, DES_ENCRYPT);
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
	// ex: DES_ncbc_encrypt(in, out, hello.size(), &keysched, &ivec, DES_ENCRYPT);
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