// Auxiliar Lists
CxList methodDecls = Find_MethodDecls();
CxList parameters = Find_Param();
CxList validMethods = All.NewCxList();
CxList statementCollections = Find_StatementCollection();
CxList returnStmts = Find_ReturnStmt();
CxList iterationStmts = Find_IterationStmt();
CxList unknowRefs = Find_UnknownReference();
CxList switchStmts = Find_SwitchStmt();
CxList cases = All.FindByType(typeof(Case));
CxList expressions = Find_Expressions();
CxList paramDecls = Find_ParamDecl();

CxList AllSwitchCases = cases.FindByFathers(switchStmts);
CxList AllSwitchVariable = unknowRefs.FindByFathers(switchStmts);

foreach (CxList methodDecl in methodDecls)
{
	//Gets all the parameters from the methods
	CxList methodParameters = All.GetParameters(methodDecl);
	
	//1st condition: the method needs to have atleast 2 parameters
	if(methodParameters.Count == 0)
	{
		continue;
	}
	
	CxList switchCases = AllSwitchCases.GetByAncs(methodDecl);
	CxList switchVariable = AllSwitchVariable.GetByAncs(methodDecl);

	bool foundUnsafe = false;
	CxList safeParameters = All.NewCxList();
	
	if(cases.Count == 0)
	{
		continue;
	}
	
	CxList conditionalVariablesFromParameter = switchVariable.FindAllReferences(methodParameters);
	
	foreach(CxList switchCase in switchCases)
	{
		Case caseObj = switchCase.TryGetCSharpGraph<Case>();
		if(caseObj.IsDefault)
		{
			continue;
		}
		
		CxList caseCondition = expressions.FindByFathers(switchCase);
		
		if(caseCondition.FindByAbstractValue(x => !(x is AnyAbstractValue)).Count == 0)
		{
			foundUnsafe = true;
			break;			
		}
			
		//2nd condition: the iterator needs to have an if. And the parameter from methods needs to be part of the condition
		if(conditionalVariablesFromParameter.Count == 0)
		{
			continue;
		}
			
		CxList validCase = returnStmts.FindByFathers(caseCondition);

		//3rd condition: the conditionalStmt needs to have a return statement inside.
		if(validCase.Count == 0)
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
	CxList thisMethodInvocations = Find_Methods().FindAllReferences(methodDecl);
	validMethods.Add(All.GetParameters(thisMethodInvocations, index) - parameters);
	result.Add(validMethods);
}


// Returns the list of sanitizers
result.Add(validMethods);