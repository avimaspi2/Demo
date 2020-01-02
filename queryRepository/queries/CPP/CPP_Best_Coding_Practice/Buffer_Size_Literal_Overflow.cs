// Check for C-arrays whose size specified as a literal instead of a macro or a constant.
CxList num = Find_Integer_Literals(); 
CxList arraySizeLiterals = Find_Array_Size_Literals();
CxList classes = Find_Classes();

CxList conditions = Find_Conditions();
CxList potenialOverflow = conditions.FindByName("<="); // wrong index\size check
CxList number = num.FindByShortName(arraySizeLiterals);

CxList indexers = All.FindByType(typeof(IndexerRef));
CxList relevantReferences = Find_UnknownReference();
CxList declarators = Find_All_Declarators();
relevantReferences.Add(indexers);
relevantReferences.Add(declarators);

CxList referencesFound = relevantReferences.FindAllReferences(arraySizeLiterals.GetAncOfType(typeof(Declarator)));
CxList referencesOfArraySizeLiterals = number.FindByShortName(arraySizeLiterals);


foreach(CxList arraySizeLiteral in arraySizeLiterals)
{	
	// Find the literal declarator references
	CxList literalDeclaratorReferences = arraySizeLiteral.GetAncOfType(typeof (Declarator));
	literalDeclaratorReferences = referencesFound.FindAllReferences(literalDeclaratorReferences);	
	CxList allInstancesOfArraySizeLiteral = referencesOfArraySizeLiterals.FindByShortName(arraySizeLiteral);
	CxList curScope = arraySizeLiteral.GetAncOfType(typeof(MethodDecl));
	if (curScope.Count == 0)
	{
		curScope = classes.GetClass(arraySizeLiteral);
	}
	// Find Condition related to the literal size of the array
	CxList conditionScope = potenialOverflow.GetByAncs(curScope);
	CxList literalsInScope = allInstancesOfArraySizeLiteral.GetByAncs(conditionScope);
	if (literalsInScope.Count > 0)
	{		
		// Find the literalsInScope IndexerRef
		CxList literalsInScopeIndexerRef = literalsInScope.GetAncOfType(typeof (IndexerRef));
		CxList allJointRef = literalDeclaratorReferences.FindAllReferences(literalsInScopeIndexerRef);	
		if(allJointRef.Count > 0){
			result.Add(arraySizeLiteral.ConcatenateAllPaths(literalsInScope, false));
		}
	}
}