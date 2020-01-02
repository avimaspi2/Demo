/**
	The following query will find all usages of salts without a good size. 
	This query finds all the salts that have a predictable size being and that are being used in the OpenSSL PBKDF2.

**/

if(param.Length == 1)
{
	// Limit from where we consider it safe
	int saltSize = (int) param[0];

	CxList methods = Find_Methods();

	// Find all salts being used in OpenSSL's PBKDF2 calls
	CxList openSSLPBKDF2Executions = methods.FindByShortName("PKCS5_PBKDF2_HMAC*");
	CxList PBKDF2SaltParameters = All.GetParameters(openSSLPBKDF2Executions, 3);
	CxList PBKDF2SaltSize = Find_Unknown_References().GetParameters(PBKDF2SaltParameters);

	// Find all salts with small size:
	IAbstractValue intervalZeroToSaltSize = new IntegerIntervalAbstractValue(0, saltSize - 1); 

	CxList sizeOfSalts = All.NewCxList();

	//Handles the case where the salt size is set with sizeof()
	//ex: PKCS5_PBKDF2_HMAC_SHA1(pwd, strlen(pwd), salt_value, sizeof(salt_value), 20000, KEK_KEY_LEN, out)
	foreach(CxList sSize in PBKDF2SaltSize.GetParameters(methods.FindByShortName("sizeof"), 0))
	{
		CxList saltReferences = All.FindAllReferences(sSize);
		CxList randBytesMethods = methods.FindByShortName("RAND_bytes");
		CxList randParameter = randBytesMethods.FindByParameters(saltReferences);
		CxList randNumBytes = All.GetParameters(randParameter, 1);
			
		CxList tempSalts = randNumBytes.FindByAbstractValue(abstractValue => abstractValue.IncludedIn(intervalZeroToSaltSize, false));
	
		sizeOfSalts.Add(sSize);
	
		if(tempSalts.Count > 0)
		{	
			result.Add(tempSalts.ConcatenatePath(sSize.DataInfluencedBy(saltReferences.GetParameters(randParameter, 0)), false));
			
		}
	}		

	CxList smallSalts = PBKDF2SaltSize.FindByAbstractValue(abstractValue => {                          
	
		ObjectAbstractValue objectAbstractValue = (abstractValue as ObjectAbstractValue);
	
		int count = 0;
		
		if( objectAbstractValue != null) 
		{
			foreach(string key in objectAbstractValue.FieldNames) count++;
		}
		
		IAbstractValue currentSaltSize = new IntegerIntervalAbstractValue(count, count); 
		
		return currentSaltSize.IncludedIn(intervalZeroToSaltSize, false);
		});

	smallSalts.Add(PBKDF2SaltParameters.FindByAbstractValue(abstractValue => abstractValue.IncludedIn(intervalZeroToSaltSize, false)));


	// Return the flow from variable declaration until the weak salt usage:
	result.Add(smallSalts.DataInfluencingOn(openSSLPBKDF2Executions));
	
}