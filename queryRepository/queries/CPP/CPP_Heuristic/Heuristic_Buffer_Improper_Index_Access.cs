CxList arrayCreateList = Find_ArrayCreateExpr();
CxList declarators = Find_Declarators();
CxList unknRefs = Find_Unknown_References();
CxList indexes = Find_IndexerRefs();

CxList inputs = Find_Inputs();
CxList allRefsInputs = All.FindAllReferences(inputs);
CxList indexesUnkRef = unknRefs.GetByAncs(indexes);
CxList indexesUnkAllRefs = All.FindAllReferences(indexesUnkRef);
CxList indexesNoInput = indexesUnkRef - allRefsInputs;
CxList indexesInfluenced = indexesNoInput.DataInfluencedBy(inputs);

CxList conditions = Find_Conditions();
CxList binaryExpressions = Find_BinaryExpressions();
List<string> relevantBinary = new List<string>() {"<", "<=", ">", ">="}; 
CxList relevantBinaryList = binaryExpressions.FindByShortNames(relevantBinary);
CxList binarysInIfs = relevantBinaryList.GetByAncs(conditions);
CxList inConditions = indexesUnkAllRefs.GetByAncs(conditions);
CxList sanitizedRefsNodes = binarysInIfs.DataInfluencedBy(inConditions).GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
CxList sanitizedRefs = All.FindAllReferences(sanitizedRefsNodes).GetAncOfType(typeof(IndexerRef));
CxList sanitizedIndexesUnkRef = unknRefs.GetByAncs(sanitizedRefs);

CxList indexesInfluencedAndNotSanitized = indexesInfluenced - sanitizedIndexesUnkRef;

CxList nodes = All.NewCxList();

//The purpose of this lambda is to not duplicate code inside the foreach
//This lambda receives the current node in the foreach and returns its IntegerIntervalAbstractValue
Func <CxList, IntegerIntervalAbstractValue> calcAbsValue = arrayReference => {
	IntegerIntervalAbstractValue arrayReferenceValue = null;
	CxList definition = declarators.FindDefinition(arrayReference);
	CxList arrayCreate = arrayCreateList.FindByFathers(definition);
	ArrayCreateExpr arrayDef = arrayCreate.TryGetCSharpGraph<ArrayCreateExpr>();
	if(arrayDef != null){
		if(arrayDef.Sizes.Count != 0){
			if (arrayDef.Sizes[0] != null && arrayDef.Sizes is ExpressionCollection) {
				Expression col = arrayDef.Sizes[0] as Expression;
				if(col is IntegerLiteral) {
					IntegerLiteral integer = col as IntegerLiteral;
					arrayReferenceValue = new IntegerIntervalAbstractValue(0, integer.Value);
				}
			}
		}
		else if(arrayDef.Initializer != null && arrayDef.Initializer is AbstractCollectionInitializer) {
			AbstractCollectionInitializer values = arrayDef.Initializer as AbstractCollectionInitializer;
			if(values.InitialValues != null) {
				ExpressionCollection col = values.InitialValues as ExpressionCollection;
				arrayReferenceValue = new IntegerIntervalAbstractValue(col.Count, col.Count);
			}
		}
	}
	return arrayReferenceValue;
	};

foreach(CxList arrayRef in indexes) {
	try {
		IndexerRef indexer = arrayRef.TryGetCSharpGraph<IndexerRef>();
		Expression arrayIndexer = indexer.Indices[0] as Expression;
		if (arrayIndexer is IntegerLiteral) {
			continue;
		}
		CxList arrayReference = unknRefs.FindByFathers(arrayRef);
		
		UnknownReference array = arrayReference.TryGetCSharpGraph<UnknownReference>();
		if(arrayIndexer != null && array != null){
			IntegerIntervalAbstractValue arrayReferenceValue = null;
			long arrayValueMax = 0;
			long arrayValueMin = 0;
			if(array.AbsValue is ObjectAbstractValue) {
				ObjectAbstractValue arrayAbstractValue = array.AbsValue as ObjectAbstractValue;
				if(arrayAbstractValue != null) {
					//char msg[13] = {'h','l'};
					if(arrayAbstractValue.AllocatedSize != null) {
						arrayValueMax = arrayAbstractValue.AllocatedSize.UpperIntervalBound.GetValueOrDefault();
						arrayValueMin = arrayAbstractValue.AllocatedSize.LowerIntervalBound.GetValueOrDefault();
						arrayReferenceValue = new IntegerIntervalAbstractValue(arrayValueMin, arrayValueMax);
					}
						//char msg[] = {'h','l'};
					else{
						arrayReferenceValue = calcAbsValue(arrayReference);
					}
				}	
			}
				//Loops
			else if (array.AbsValue is AnyAbstractValue){
				arrayReferenceValue = calcAbsValue(arrayReference);
			}
			if(arrayReferenceValue == null) {
				if(!(arrayIndexer.AbsValue is IntegerIntervalAbstractValue)) {
					CxList indexesUnkRefIn = unknRefs.FindByFathers(arrayRef);
					CxList common = indexesUnkRefIn * indexesInfluencedAndNotSanitized;
					if(common.Count > 0) {
						nodes.Add(arrayRef);
					}
				}
			}
			else if(arrayReferenceValue != null) {
				if(arrayIndexer.AbsValue != null && !(arrayIndexer.AbsValue is IntegerIntervalAbstractValue)) {
					CxList indexesUnkRefIn = unknRefs.FindByFathers(arrayRef);
					CxList common = indexesUnkRefIn * indexesInfluencedAndNotSanitized;
					if(common.Count > 0) {
						nodes.Add(arrayRef);
					}
				}
			}
		}
	}
	catch(Exception e){
		cxLog.WriteDebugMessage(e.Message);
	}
}

//Creates flow with indexerRefs that are influenced by user input
CxList unkRefsIndexer = unknRefs.FindByFathers(nodes);
CxList inputsNodes = unkRefsIndexer.DataInfluencedBy(inputs);
CxList inputNodesUnkRefs = inputsNodes.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
CxList inputNodeIndexerRef = inputNodesUnkRefs.GetAncOfType(typeof(IndexerRef));
nodes -= inputNodeIndexerRef;
nodes.Add(inputsNodes);
result = nodes;