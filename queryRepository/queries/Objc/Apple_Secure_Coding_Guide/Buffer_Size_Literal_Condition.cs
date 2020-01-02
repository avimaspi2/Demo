///////////////////////////////////////////////////////////////////////
//				Buffer_Size_Literal_Condition
//
// This query finds arrays that their size is a literal,
// there is a use of that literal in a condition 
// and inside the condition there is a use of the array. 
//////////////////////////////////////////////////////////////////////

// Find array size literals
CxList arrays = Find_ArrayCreateExpr();
CxList num = Find_IntegerLiterals();
CxList arraySizeLiterals = num.GetByAncs(arrays);

// Find numbers that are part of a condition
CxList classes = Find_Classes();
CxList conditions = Find_Condition();
CxList number = num.FindByName(arraySizeLiterals);

foreach(CxList arraySizeLiteral in arraySizeLiterals)
{
	// Check if the number in the array declaration is not part of a binary expression
	CxList binary = arraySizeLiteral.GetFathers().FindByType(typeof(BinaryExpr));
	if(binary.Count > 0)
		continue;
	
	// Find the literal declarator references
	CxList literalDeclaratorReferences = arraySizeLiteral.GetAncOfType(typeof (Declarator));
	literalDeclaratorReferences = All.FindAllReferences(literalDeclaratorReferences);
	
	CxList allInstancesOfArraySizeLiteral = number.FindByName(arraySizeLiteral);
	
	CxList curScope = arraySizeLiteral.GetAncOfType(typeof(StatementCollection));
	if (curScope.Count == 0)
	{
		curScope = classes.GetClass(arraySizeLiteral);
	}
 
	// Find Condition related to the literal size of the array  
	CxList conditionScope = conditions.GetByAncs(curScope);
	CxList conditionScopeChildrens = All.GetByAncs(conditionScope);
	foreach(CxList condition in conditionScope){
		CxList literalsInScope = allInstancesOfArraySizeLiteral.GetByAncs(condition);
	
		// Find declarator references inside the condition scope
		CxList declaratorInScope = literalDeclaratorReferences.GetByAncs(condition.GetFathers());
		declaratorInScope -= conditionScopeChildrens.GetByAncs(condition);

		if (literalsInScope.Count > 0 && declaratorInScope.Count > 0)
		{
			result.Add(arraySizeLiteral.ConcatenateAllPaths(literalsInScope, false));
		}
	}
}