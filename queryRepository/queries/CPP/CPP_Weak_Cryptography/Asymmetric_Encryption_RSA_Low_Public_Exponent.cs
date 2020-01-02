/* The following query will search for generation of RSA public keys using low exponents (< 65537) */

CxList methodInvokes = Find_Methods();
CxList objectCreations = Find_ObjectCreations();
CxList intLiterals = Find_IntegerLiterals();
CxList stringLiterals = Find_String_Literal();

CxList RSAPublicKeyExponentParams = All.NewCxList();

// Botan
RSAPublicKeyExponentParams.Add(All.GetParameters(objectCreations.FindByShortName("RSA_PublicKey"), 1));

// Crypto++
RSAPublicKeyExponentParams.Add(All.GetParameters(methodInvokes.FindByMemberAccess("RSAFunction.Initialize"), 1));
RSAPublicKeyExponentParams.Add(All.GetParameters(methodInvokes.FindByMemberAccess("RSAFunction.SetPublicExponent"), 0));

// OpenSSL
RSAPublicKeyExponentParams.Add(All.GetParameters(methodInvokes.FindByShortName("RSA_generate_key_ex"), 2));
RSAPublicKeyExponentParams.Add(All.GetParameters(methodInvokes.FindByShortName("RSA_generate_key"), 1));

CxList lowPublicExponent = intLiterals.FindByAbstractValue(i => {
	if (i is IntegerIntervalAbstractValue)
	{
		IntegerIntervalAbstractValue minVal = new IntegerIntervalAbstractValue(65537);
		return (i as IntegerIntervalAbstractValue).LessThan(minVal) is TrueAbstractValue;
	}
	return false;
});

// These libraries accept exponents to be passed as strings (decimal, hexadecimal)
lowPublicExponent.Add(stringLiterals.FindByAbstractValue(s => {
	if (s is StringAbstractValue)
	{
		int intVal;
		string content = (s as StringAbstractValue).Content;
		
		bool isDec = Int32.TryParse(content, out intVal);
		bool isHex = content.StartsWith("0x") && 
			Int32.TryParse(content.Substring(2), System.Globalization.NumberStyles.HexNumber, null, out intVal);
		
		if (isDec || isHex)
		{
			IntegerIntervalAbstractValue minVal = new IntegerIntervalAbstractValue(65537);
			IntegerIntervalAbstractValue actualVal = new IntegerIntervalAbstractValue(intVal);
			return actualVal.LessThan(minVal) is TrueAbstractValue;
		}
	}
	return false;
}));

result = lowPublicExponent.DataInfluencingOn(RSAPublicKeyExponentParams);