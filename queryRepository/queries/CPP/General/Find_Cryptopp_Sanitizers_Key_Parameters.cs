result = All.GetParameters(Find_Methods().FindByShortNames(new List<string> {
		"GeneratePrivateKey", "GenerateStaticPrivateKey", "GenerateEphemeralPrivateKey"
		}), 1) - Find_Parameters();

result.Add(All.GetParameters(Find_Methods().FindByShortName("GenerateBlock"), 0) - Find_Parameters());

CxList generatePublicKey = Find_Methods().FindByShortNames(new List<string> {
		"GeneratePublicKey", "GenerateKeyPair", "GenerateStaticPublicKey", "GenerateStaticKeyPair",
		"GenerateEphemeralPublicKey", "GenerateEphemeralKeyPair"});

result.Add(All.GetParameters(generatePublicKey, 1) - Find_Parameters());
result.Add(All.GetParameters(generatePublicKey, 2) - Find_Parameters());