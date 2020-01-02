// Auxiliar Lists
CxList methodDecls = Find_MethodDecls();
CxList parameters = Find_Param();
CxList validMethods = All.NewCxList();
CxList statementCollections = Find_StatementCollection();
CxList returnStmts = Find_ReturnStmt();
CxList iterationStmts = Find_IterationStmt();
CxList unknowRefs = Find_UnknownReference();
CxList switchStmts = Find_SwitchStmt();
CxList cases = Find_Cases();
CxList binaries = Find_BinaryExpr();
CxList methods = Find_Methods();
CxList expressions = Find_Expressions();
CxList ifs = Find_Ifs();

//Iterate through the method decls to check the result validation conditions
foreach (CxList methodDecl in methodDecls)
{
	//Gets all the parameters from the methods
	CxList methodParameters = All.GetParameters(methodDecl);
	
	//1st condition: the method needs to have atleast 2 parameters
	if(methodParameters.Count == 0)
	{
		continue;
	}
	
	CxList conditionalIfStmt = expressions.FindByFathers(ifs.GetByAncs(methodDecl));
		
	CxList conditionalIfStmtBinaryOperators = (conditionalIfStmt * binaries).GetByBinaryOperator(BinaryOperator.IdentityEquality);
	conditionalIfStmtBinaryOperators.Add((conditionalIfStmt * methods).FindByShortName("Equals", false));

	bool foundUnsafe = false;
	CxList safeParameters = All.NewCxList();

	if(conditionalIfStmtBinaryOperators.Count == 0)
	{
		continue;
	}
	
	foreach(CxList conditionalObject in conditionalIfStmtBinaryOperators)
	{
		CxList conditionalVariables = All.FindByFathers(conditionalObject);
		

		if(conditionalVariables.FindByAbstractValue(x => !(x is AnyAbstractValue)).Count == 0)
		{
			foundUnsafe = true;
			break;			
		}
		
		CxList conditionalVariablesFromParameter = conditionalVariables.FindAllReferences(methodParameters);
	
		//2nd condition: the iterator needs to have an if. And the parameter from methods needs to be part of the condition
		if(conditionalVariablesFromParameter.Count == 0)
		{
			continue;
		}
			
		CxList validIf = returnStmts.FindByFathers(statementCollections.FindByFathers(conditionalIfStmt.GetFathers()));
			
		//3rd condition: the conditionalStmt needs to have a return statement inside.
		if(validIf.Count == 0)
		{
			continue;
		}
			
		safeParameters.Add(conditionalVariablesFromParameter);
		
		
	}
	
	if(foundUnsafe)
	{
		continue;
	}
	
	CxList relevantParameter = methodParameters.FindAllReferences(safeParameters);
				
	if(relevantParameter.Count == 0)
	{
		continue;
	}
	
	int index = relevantParameter.GetIndexOfParameter();
	CxList thisMethodInvocations = methods.FindAllReferences(methodDecl);

	validMethods.Add(All.GetParameters(thisMethodInvocations, index) - parameters);
	result.Add(validMethods);
	
}