// Auxiliar Lists
CxList methodDecls = Find_MethodDecls();
CxList parameters = Find_Param();
CxList validMethods = All.NewCxList();
CxList statementCollections = Find_StatementCollection();
CxList returnStmts = Find_ReturnStmt();
CxList iterationStmts = Find_IterationStmt();
CxList unknowRefs = Find_UnknownReference();
CxList binaries = Find_BinaryExpr();
CxList methods = Find_Methods();

//Checks if a custom method is inside an array of preset values
// Example: 
//func stringInSlice(a string, list[]string) bool {
//	for b := range list {
//		if b == a {
//			return true
//		}
//	}
//	return false
//}

//Iterate through the method decls to check the result validation conditions
foreach (CxList methodDecl in methodDecls)
{
	//Gets all the parameters from the methods
	CxList methodParameters = All.GetParameters(methodDecl);

	//1st condition: the method needs to have atleast 2 parameters
	if(methodParameters.Count < 2)
	{
		continue;
	}
	
	CxList iterationInsideMethod = iterationStmts.GetByAncs(methodDecl);
	
	CxList iterationParameters = unknowRefs.FindByFathers(All.FindByFathers(iterationInsideMethod));
	
	//2nd condition: the method should have an iterator inside the method
	if(iterationInsideMethod.Count == 0)
	{
		continue;
	}
	
	CxList conditionalIfStmt = Find_Expressions().FindByFathers(Find_Ifs().GetByAncs(iterationInsideMethod));
	
	CxList conditionalIfStmtBinaryOperators = (conditionalIfStmt * binaries).GetByBinaryOperator(BinaryOperator.IdentityEquality);
	conditionalIfStmtBinaryOperators.Add((conditionalIfStmt * methods).FindByShortName("Equals", false));
	
	CxList conditionalVariables = All.FindByFathers(conditionalIfStmtBinaryOperators);
	CxList conditionalVariablesFromParameter = conditionalVariables.FindAllReferences(methodParameters);
	
	//3rd condition: the iterator needs to have an if. And the parameter from methods needs to be part of the condition
	if(conditionalVariablesFromParameter.Count == 0)
	{
		continue;
	}
	
	//4th condition: the iterator parameter needs to be part of the condition
	if(conditionalVariables.FindAllReferences(iterationParameters).Count == 0)
	{
		continue;
	}
	CxList validIf = returnStmts.FindByFathers(statementCollections.FindByFathers(conditionalIfStmt.GetFathers()));
		
	//5th condition: the conditionalStmt needs to have a return statement inside.
	if(validIf.Count == 0)
	{
		continue;
	}
	
	CxList relevantParameter = methodParameters.FindAllReferences(conditionalVariablesFromParameter);

	if(relevantParameter.Count == 0)
	{
		continue;
	}
	
	int index = relevantParameter.GetIndexOfParameter();

	CxList thisMethodInvocations = Find_Methods().FindAllReferences(methodDecl);
	validMethods.Add(All.GetParameters(thisMethodInvocations, index) - parameters);
	result.Add(validMethods);
}

result = validMethods;