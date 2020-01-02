/**
	This query searches for bad key sizes in Crypto++'s RSA functions
*/

/**** Constant data ****/
long RSA_MINIMUM_KEY_SIZE = 2048;

/**** Gathering vulnerabilities ****/
// Find numbers smaller than rsa minimum key size
CxList rsaBadSizes = Find_Number_Out_Of_Range(RSA_MINIMUM_KEY_SIZE -1, Int64.MaxValue);

// Find bad usages of RSA keys
CxList rsaKeyGenerators = Find_Methods().FindByMemberAccess("InvertibleRSAFunction.GenerateRandomWithKeySize");
rsaKeyGenerators.Add(Find_Methods().FindByMemberAccess("PrivateKey.GenerateRandomWithKeySize"));
CxList rsaKeyGeneratorParameters = All.GetParameters(rsaKeyGenerators, 1);
CxList rsaPotentialVulnerableKeys = rsaBadSizes.FindByFathers(rsaKeyGeneratorParameters);


// Find Eliptic Curve usages
List<string> eccBlackList = new List<string>(new string[] {
	"secp112r1", "secp128r1", "secp128r2",
	"secp160k1", "secp160r1","secp160r2",
	"secp192k1", 
	"secp224k1", "secp224r1",
	"brainpoolP160r1", "brainpoolP192r1", "brainpoolP224r1"
	});
CxList eccCurveConstants = Find_Methods().FindByShortNames(eccBlackList);
CxList elipticCurveKeyGenerators = Find_Methods().FindByMemberAccess("PrivateKey.Initialize");

/**** Returning sinks ****/
result.Add(rsaPotentialVulnerableKeys
	.InfluencingOn(rsaKeyGenerators)
	.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow));
result.Add(eccCurveConstants
	.InfluencingOn(elipticCurveKeyGenerators)
	.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow));