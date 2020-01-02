/// <summary>
/// This query returns flows from methods that retrieve S3 objects data
/// and are influenced by interactive inputs, to interactive outputs
/// </summary>
CxList interactiveInputs = Find_Interactive_Inputs();
CxList objCreations = Find_ObjectCreations(); 
CxList unkRefs = Find_UnknownReference();

string[] classNames = new string[] {"AmazonS3", "AmazonS3Client", "S3Object", "TransferManagerBuilder"};

CxList methods = unkRefs.FindByTypes(classNames).GetMembersOfTarget();

CxList amazonS3Methods = methods.FindByShortNames(new List<string>{"getObject", "getObjectAsString", 
		"getObjectMetadata", "getObjectContent", "download", "downloadDirectory"});
amazonS3Methods.Add(objCreations.FindByShortName("GetObjectRequest"));

// Get sanitizers
CxList sanitizers = Find_CastExpr();
List < string > converters = new List<string> {"Integer", "Float", "Double", "Boolean"};
List <string> converterMethods = new List<string> {"parseInt", "parseFloat", "parseDouble", "parseBoolean"};
sanitizers.Add(unkRefs.FindByShortNames(converters).GetMembersOfTarget().FindByShortNames(converterMethods));

// Get interactive inputs influencing on methods and not sanitized
CxList inputsInfluencingOnMethods = interactiveInputs.InfluencingOnAndNotSanitized(amazonS3Methods, sanitizers);

CxList interactiveOutputs = Find_Interactive_Outputs();

// Get interactive outputs influenced by inputs that, in turn, influence methods
CxList outputsInfluencedByInputs = interactiveOutputs.InfluencedBy(inputsInfluencingOnMethods);

// Get methods influenced by onputs and not sanitized
CxList methodsInfluencedByInputs = amazonS3Methods.InfluencedByAndNotSanitized(interactiveInputs, sanitizers);

// Get outputs influenced by interactive inputs but not by methods, these are removed from the results below
CxList outputsInflByInputsButNotByMethods = interactiveOutputs.InfluencedByAndNotSanitized(inputsInfluencingOnMethods, methodsInfluencedByInputs);

result = (outputsInfluencedByInputs - outputsInflByInputsButNotByMethods).ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);