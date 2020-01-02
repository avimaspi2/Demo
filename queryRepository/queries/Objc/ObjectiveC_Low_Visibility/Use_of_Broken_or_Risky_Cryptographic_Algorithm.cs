//with the usage of import CommonCrypto
CxList methods = Find_Methods();
CxList weakEncryptionMethods = methods.FindByShortNames(new List <string>{"CC_MD2", "CC_MD2_Final", "CC_MD4", 
		"CC_MD4_Final", "CC_MD5", "CC_MD5_Final", "CC_SHA1", "CC_SHA1_Final", "CC_SHA224", "CC_SHA224_Final"});

CxList weakEncryption = Find_UnknownReference();
weakEncryption = weakEncryption.FindByShortNames(new List <string>{"kCCOptionECBMode ", "kCCAlgorithmDES", 
		"kCCAlgorithm3DES", "kCCAlgorithmRC2", "kCCAlgorithmRC4"});

CxList cccryptParams = All.GetParameters(methods.FindByShortName("CCCrypt*"), 1);
CxList algorithmsFlowToParams = weakEncryption.InfluencingOn(cccryptParams);
algorithmsFlowToParams.Add(weakEncryption * cccryptParams);

result = weakEncryptionMethods;
result.Add(algorithmsFlowToParams);