/// <summary>
/// The following query will find insufficient output length for hash.
/// The sufficient output length is variable through time, so this query receives that output length as parameter, that is required.
/// This query finds all output length that have a predictable size and that are being used in the cryptopp's lib. 
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
	
	// RandomNumberGenerator::GenerateBlock		
	// ex: crypto_generichash(hash, sizeof hash, MESSAGE, MESSAGE_LEN, key, sizeof key);
	outputLengthNotInRange.Add(Find_Not_In_Range("cryptopp/*", "GenerateBlock", 1, outputLength, null));
	// StreamTransformation::ProcessLastBlock
	// ex: 
	outputLengthNotInRange.Add(Find_Not_In_Range("cryptopp/*", "ProcessLastBlock", 1, outputLength, null));
	// StreamTransformation::ProcessData
	// ex: 
	outputLengthNotInRange.Add(Find_Not_In_Range("cryptopp/*", "ProcessData", 2, outputLength, null));
	// StreamTransformation::ProcessString
	// ex: 
	outputLengthNotInRange.Add(Find_Not_In_Range("cryptopp/*", "ProcessString", 2, outputLength, null));
	// BlockTransformation::AdvancedProcessBlocks
	// ex: 
	outputLengthNotInRange.Add(Find_Not_In_Range("cryptopp/*", "AdvancedProcessBlocks", 3, outputLength, null));	
	
	// Get known output length list (IntegerIntervalAbstractValue)
	CxList knownOutputLength = outputLengthNotInRange.FindByAbstractValue(x => x is IntegerIntervalAbstractValue);
				
	// Get unknown output length list (AnyAbstractValue and ObjectAbstractValue)
	CxList anyAbstractValue = outputLengthNotInRange.FindByAbstractValue(x => x is AnyAbstractValue);
	CxList objectAbstractValue = outputLengthNotInRange.FindByAbstractValue(x => x is ObjectAbstractValue);
	CxList unknownOutputLength = anyAbstractValue + objectAbstractValue;
		
	// Good constants based on their size 
	List<string> goodConstants = new List<string>{
		// AES (AES::*)
		"AES.MAX_KEYLENGTH",				// size: 32
		
		// Blowfish (Blowfish::*)
		"Blowfish.MAX_KEYLENGTH",			// size: 56
		
		// Threefish (Threefish256::* | Threefish512::* | Threefish1024::*)
		"Threefish256.DEFAULT_KEYLENGTH", 	// size: 32
		"Threefish256.MIN_KEYLENGTH", 		// size: 32
		"Threefish256.MAX_KEYLENGTH", 		// size: 32
		"Threefish256.BLOCKSIZE", 			// size: 32
		"Threefish512.DEFAULT_KEYLENGTH", 	// size: 64
		"Threefish512.MIN_KEYLENGTH", 		// size: 64
		"Threefish512.MAX_KEYLENGTH", 		// size: 64
		"Threefish512.BLOCKSIZE", 			// size: 64
		"Threefish1024.DEFAULT_KEYLENGTH",	// size: 128
		"Threefish1024.MIN_KEYLENGTH", 		// size: 128
		"Threefish1024.MAX_KEYLENGTH", 		// size: 128
		"Threefish1024.BLOCKSIZE", 			// size: 128
	};
	
	
	anyAbstractValue = anyAbstractValue.FindByType(typeof (MemberAccess)) + anyAbstractValue.FindByType(typeof (UnknownReference));
	
	// search good (known) constant sizes
	CxList goodOutputLength = All.NewCxList();
	foreach(String constant in goodConstants) 
	{
		goodOutputLength.Add(anyAbstractValue.FindByMemberAccess(constant));
	}
	CxList badOutputLength = anyAbstractValue - goodOutputLength;
	CxList badOutputLengthDefinitions = All.FindDefinition(knownOutputLength);
	// Add bad and unknown hard coded constant to result
	// ex: rng.GenerateBlock(salt, AES::DEFAULT_KEYLENGTH);
	result.Add(badOutputLength);
			
	// Get the unknown output length definitions
	CxList outputLengthDefinitions = All.FindAllReferences(unknownOutputLength.GetTargetOfMembers()).FindByAssignmentSide(CxList.AssignmentSide.Left);
	outputLengthDefinitions.Add(All.FindAllReferences(unknownOutputLength).FindByAssignmentSide(CxList.AssignmentSide.Left));
	
	// Get the known output length definitions
	CxList knownOutputLengthDefinitions = All.FindDefinition(knownOutputLength);

	// Add known output length 				
	result.Add(knownOutputLength.DataInfluencedBy(knownOutputLengthDefinitions));
		
	// Add hard coded output length values
	// ex: rng.GenerateBlock(salt1, 16);
	result.Add(knownOutputLength - result);

	// Get the SecByteBlock size allocation used as output length
	CxList secByteBlock = outputLengthDefinitions.FindByType("SecByteBlock");
	CxList secByteBlockAssigner =  secByteBlock.GetAssigner();
	// ex: SecByteBlock salt(64);
	CxList secByteBlockParameters = All.GetParameters(Filter_By_Parameter_Count(secByteBlockAssigner, 2), 1);
	// ex: SecByteBlock salt3(0x00, AES::DEFAULT_KEYLENGTH);
	secByteBlockParameters.Add(All.GetParameters(Filter_By_Parameter_Count(secByteBlockAssigner, 1), 0));
	// Remove duplicated values
	secByteBlockParameters -= paramList;
	
	// Finds all output length (SecByteBlock) with a lenght < outputLength;
	CxList secByteBlockSmall = secByteBlockParameters.FindByAbstractValue(abstractValue => abstractValue.IncludedIn(intervalZeroToOutputLength, false));
	result.Add(outputLengthNotInRange.DataInfluencedBy(secByteBlockSmall));
	
	// Finds all output length (SecByteBlock) with a bad or unknown hard coded constant
	anyAbstractValue = secByteBlockParameters.FindByType(typeof (MemberAccess)) + secByteBlockParameters.FindByType(typeof (UnknownReference));
	
	// Search good (known) constant sizes
	goodOutputLength = All.NewCxList();
	foreach(String constant in goodConstants) 
	{
		goodOutputLength.Add(anyAbstractValue.FindByMemberAccess(constant));
	}
	badOutputLength = anyAbstractValue - goodOutputLength;
	result.Add(outputLengthNotInRange.DataInfluencedBy(badOutputLength));
}