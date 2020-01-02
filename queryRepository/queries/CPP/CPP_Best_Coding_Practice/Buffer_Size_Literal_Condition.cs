///////////////////////////////////////////////////////////////////////
//  Buffer_Size_Literal_Condition
//
// This query find arrays that their size is a literal,
// there is a use with that literal in a condition 
// and inside the condition there is a use with that array. 

CxList integerLiteral = Find_Integer_Literals();
CxList integersAsParameters = integerLiteral.GetByAncs(Find_Methods());
CxList ArrayDefinition = Find_ArrayCreateExpr();
CxList unkRefs = Find_Unknown_References();
CxList foundSizeInt = All.NewCxList();
//find arrays that are initialized with an int as a size
foreach(CxList arrayDef in ArrayDefinition)
{
	try
	{
		ArrayCreateExpr g = arrayDef.TryGetCSharpGraph<ArrayCreateExpr>();
		if(g == null || g.Sizes == null)
		{
			continue;
		}
		ExpressionCollection arraySizes = g.Sizes;
		foreach(Expression size in arraySizes)
		{
			if (size != null)
			{
				foundSizeInt.Add(integerLiteral.GetByAncs(All.FindById(size.NodeId)));
			}
		}
	}
	catch(Exception e)
	{
		cxLog.WriteDebugMessage(e);
	}
}

if(foundSizeInt.Count > 0)
{
	CxList conditions = Find_Conditions();
	CxList minimizingIntegerGroup = integerLiteral.FindByShortName(foundSizeInt).GetByAncs(conditions);
	minimizingIntegerGroup -= integersAsParameters;
	CxList relevantDeclarators = ArrayDefinition.GetAncOfType(typeof(Declarator));
	CxList refsOfDefs = All.FindAllReferences(relevantDeclarators);
	CxList allReferencesOfDeclUnderCond = refsOfDefs.GetByAncs(conditions);
	allReferencesOfDeclUnderCond.Add(refsOfDefs.GetByAncs(conditions.GetFathers()));
	//find scope of the declaration of the array
	foreach(CxList size in foundSizeInt)
	{                           
		CxList scope = size.GetAncOfType(typeof(MethodDecl));
		if(scope.Count == 0)
		{
			scope = size.GetAncOfType(typeof(ClassDecl));                
		}
		CxList allUnderScope = All.GetByAncs(scope);
		CxList sameNumberFound = minimizingIntegerGroup.FindByShortName(size) * allUnderScope;                                  
		CxList refsOfArrayDef = allReferencesOfDeclUnderCond.FindAllReferences(size.GetAncOfType(typeof(Declarator)));
		//look inside conditions, that we find both the array and the number under the same condition
		CxList relevantConditions = conditions * allUnderScope;
		foreach(CxList condition in relevantConditions)
		{
			CxList allUnderCondition = All.GetByAncs(condition);
			CxList number = sameNumberFound * allUnderCondition;
			CxList arrayReference = refsOfArrayDef * allUnderCondition;	
			arrayReference.Add(refsOfArrayDef.GetByAncs(condition.GetFathers()));
			arrayReference -= unkRefs;
			if(number.Count > 0 && arrayReference.Count > 0)
			{
				result.Add(size.ConcatenateAllTargets(arrayReference));
			}                              
		}                  
	} 
}