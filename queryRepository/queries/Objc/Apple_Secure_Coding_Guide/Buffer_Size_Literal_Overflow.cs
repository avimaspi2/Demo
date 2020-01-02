// Check for C-arrays whose size specified as a literal instead of a macro or a constant.
CxList arrays = Find_ArrayCreateExpr();
CxList num = Find_IntegerLiterals();
CxList arrayChildren = num.GetByAncs(arrays);
CxList arraySizeLiterals = arrayChildren.FindByRegex(@"\[\s*\d+\s*\]");

CxList methods = Find_MethodDecls();
CxList classes = Find_Classes();
CxList conditions = Find_Condition();
CxList potentialOverflow =  conditions.FindByName("<="); // wrong index/size check

foreach(CxList arraySizeLiteral in arraySizeLiterals)
{
	CxList allInstancesOfArraySizeLiteral = num.FindByName(arraySizeLiteral).FindByRegex(@"[^\d\w]\d+[^\d\w]");

	CxList curScope = methods.GetMethod(arraySizeLiteral);
	if (curScope.Count == 0)
	{
		curScope = classes.GetClass(arraySizeLiteral);
	}
	// Find Condition related to the literal size of the array
	CxList conditionScope = potentialOverflow.GetByAncs(curScope);
	CxList literalsInScope = allInstancesOfArraySizeLiteral.GetByAncs(conditionScope);
	if (literalsInScope.Count > 0)
	{
		result += arraySizeLiteral.ConcatenateAllPaths(literalsInScope);
	}
}