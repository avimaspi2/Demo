if(param.Length == 1){
	bool checkOnlyEquals = (bool) param[0];
	
	CxList arrayCreateList = Find_ArrayCreateExpr();
	CxList declarators = Find_Declarators();
	CxList unknRefs = Find_Unknown_References();
	CxList indexes = Find_IndexerRefs();
	CxList inputs = Find_Inputs();

	CxList nodes = All.NewCxList();

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
	
			CxList arrayReference = unknRefs.FindByFathers(arrayRef);
		
			UnknownReference array = arrayReference.TryGetCSharpGraph<UnknownReference>();
			if(arrayIndexer != null && array != null){
				IntegerIntervalAbstractValue arrayReferenceValue = null;
				long arrayIndexerMin = 0;
				long arrayIndexerMax = 0;
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
				if(arrayReferenceValue != null) {
				
					if(arrayIndexer.AbsValue != null) {
						if(arrayIndexer.AbsValue is IntegerIntervalAbstractValue) {
							arrayIndexerMin = (arrayIndexer.AbsValue as IntegerIntervalAbstractValue).UpperIntervalBound.GetValueOrDefault();
							arrayIndexerMax = (arrayIndexer.AbsValue as IntegerIntervalAbstractValue).LowerIntervalBound.GetValueOrDefault();
							IntegerIntervalAbstractValue arrayIndexerValue = new IntegerIntervalAbstractValue(arrayIndexerMin, arrayIndexerMax);		
							CxList GTE = All.NewCxList();
							CxList GT = All.NewCxList();
							if(checkOnlyEquals && arrayIndexerValue.IdentityEquality(arrayReferenceValue) is TrueAbstractValue) {
										
								nodes.Add(arrayRef);
							}
							else if(! checkOnlyEquals && arrayIndexerValue.GreaterThan(arrayReferenceValue) is TrueAbstractValue){
								nodes.Add(arrayRef);
							}
						}
					}
				}
			}
		}
		catch(Exception e){
			cxLog.WriteDebugMessage(e.Message);
		}
	}
	
	result = nodes;
}
else {
	cxLog.WriteDebugMessage("This query requires one Boolean parameter. true to retrieve only arrays whose access is done with indexes equal to their allocated size and False to retrieve accesses to indexes greated than the allocated size");
}