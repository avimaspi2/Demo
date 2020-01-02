if(ReactNative_Find_Presence().Count > 0) {
	CxList inputs = Find_Personal_Info();
	CxList sanitizers = Find_Encrypt();

	CxList singleSetStorage = Find_Members("AsyncStorage.setItem");
	singleSetStorage.Add(Find_Members("AsyncStorage.mergeItem"));

	CxList multiSetStorage = Find_Members("AsyncStorage.multiSet");
	CxList multiSetParams = All.GetParameters(multiSetStorage, 0);
	CxList multiSetArrayParams = multiSetParams.FindByType(typeof(ArrayCreateExpr));
	CxList multiSetRefParams = multiSetParams.FindByType(typeof(UnknownReference)); 
	CxList multiSetArrays = Find_ArrayCreateExpr().GetByAncs(multiSetArrayParams) - multiSetArrayParams;

	// Only look to the value part of the key/value arrays

	Func<CxList, CxList> getSecondValueArrayInit = (list) => list
		.FilterByDomProperty<ArrayInitializer>(x => x.InitialValues.Count == 2)
		.CxSelectDomProperty<ArrayInitializer>(x => x.InitialValues[1]);

	multiSetArrays = multiSetArrays.CxSelectDomProperty<ArrayCreateExpr>(x => x.Initializer);
	CxList multiSetArrayOutputs = getSecondValueArrayInit(multiSetArrays);

	// Exclude inputs that are mapped to the key in a key/value array, when passing that array as param of the output

	CxList inputArrays = inputs.GetFathers().FindByType(typeof(ArrayInitializer));
	CxList inputArrayValues = getSecondValueArrayInit(inputArrays);

	CxList possibleInputs = inputs.FindByFathers(inputArrays);
	CxList badInputs = possibleInputs - (possibleInputs * inputArrayValues);
	inputs -= badInputs;

	// Outputs

	CxList outputs = All.GetParameters(singleSetStorage, 1);	
	outputs.Add(multiSetArrayOutputs);
	outputs.Add(multiSetRefParams);

	result = outputs.InfluencedByAndNotSanitized(inputs, sanitizers);

	// Add input strings passed directly to the outputs

	CxList stringLiteralInputs = inputs.FindByType(typeof(StringLiteral));
	CxList sensitiveStringsOutputs = outputs * stringLiteralInputs;
	result.Add(sensitiveStringsOutputs);

	result = result.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);
}