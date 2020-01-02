CxList allMethods = Find_Methods();
CxList integerLiterals = Find_IntegerLiterals();
CxList exprStmts = Find_ExprStmt();
CxList returnStmts = Find_ReturnStmt();
CxList conditions = Find_Conditions();

CxList errorVariables = All.FindByType("error", true);
CxList methodDeclsWithErrorReturn = All.FindByMethodReturnType("error");

//Filter methodDeclsWithErrorReturn if they are testing methods
CxList testingMethods = Find_MethodDecls_Testing();
methodDeclsWithErrorReturn = methodDeclsWithErrorReturn - testingMethods;

CxList errorHandlers = Get_Error_Handlers();
errorHandlers.Add(allMethods.FindByShortName("panic"));
errorHandlers.Add(conditions);

	
CxList unassignedErrors = All.NewCxList();
CxList methodsReturningOneSingleError = All.NewCxList();

Dictionary<string, int> methodErrorIndexPositions = new Dictionary<string, int>();
methodErrorIndexPositions.Add("\"os\".Open", 1);
methodErrorIndexPositions.Add("\"os\".Close", 0);
methodErrorIndexPositions.Add("\"os\".OpenFile", 1);
methodErrorIndexPositions.Add("\"os\".Stat", 1);
methodErrorIndexPositions.Add("\"io/ioutil\".ReadFile", 1);
methodErrorIndexPositions.Add("\"io/ioutil\".WriteFile", 0);
methodErrorIndexPositions.Add("\"io/ioutil\".ReadAll", 1);
methodErrorIndexPositions.Add("\"io/ioutil\".ReadDir", 1);
methodErrorIndexPositions.Add("\"io/ioutil\".TempDir", 1);
methodErrorIndexPositions.Add("\"io/ioutil\".TempFile", 1);
methodErrorIndexPositions.Add("\"net/http\".Get", 1);
methodErrorIndexPositions.Add("\"net/http\".Post", 1);
methodErrorIndexPositions.Add("\"net/http\".NewRequest", 1);


CxList indexerRefsMethodCalls = All.NewCxList();

foreach(var methodName in methodErrorIndexPositions)
{
	CxList methods = allMethods.FindByMemberAccess(methodName.Key);
	if(methodName.Value == 0)
	{
		methodsReturningOneSingleError.Add(methods);
	}
	else 
	{
		indexerRefsMethodCalls.Add(methods);
	}
}


foreach (CxList methodStmt in methodDeclsWithErrorReturn)
{
	MethodDecl st = methodStmt.TryGetCSharpGraph<MethodDecl>();	
	CxList methodReferences = allMethods.FindAllReferences(methodStmt);
	if(st.ReturnTypes.Count == 1)
	{
		methodsReturningOneSingleError.Add(methodReferences);
	}
	else if(st.ReturnTypes.Count > 1)
	{		
		indexerRefsMethodCalls.Add(methodReferences);
		//Adding to the dictionary just to know the error index in the code that follows
		methodErrorIndexPositions[st.FullName] = st.ReturnTypes.Count - 1;
	}
}

CxList indexerRefsReturningError = indexerRefsMethodCalls.GetFathers().FindByType(typeof(IndexerRef));

unassignedErrors.Add(methodsReturningOneSingleError.GetFathers() * exprStmts);	
errorVariables.Add(methodsReturningOneSingleError.GetAssignee());		

//Filter if defined inside a testing method
unassignedErrors = unassignedErrors - unassignedErrors.GetByAncs(testingMethods);
errorVariables = errorVariables - errorVariables.GetByAncs(testingMethods);
indexerRefsReturningError = indexerRefsReturningError - indexerRefsReturningError.GetByAncs(testingMethods);


CxList indexerRefsAlreadyProcessed = All.NewCxList();
foreach(CxList indexerRef in indexerRefsReturningError)
{
	if((indexerRefsAlreadyProcessed * indexerRef).Count > 0)
	{
		continue;
	}
	
	CSharpGraph methodInstance = indexerRef.TryGetCSharpGraph<CSharpGraph>();
	
	CxList duplicatedMethods = indexerRefsReturningError.FindByPosition(methodInstance.LinePragma.FileName, methodInstance.LinePragma.Line);
	CxList currentLiterals = integerLiterals.GetByAncs(duplicatedMethods);
	
	int position = -1;
	if(methodErrorIndexPositions.ContainsKey(methodInstance.FullName))
		position = methodErrorIndexPositions[methodInstance.FullName];
	else
		continue;
	
	CxList errorLiteral = currentLiterals.FindByShortName(methodErrorIndexPositions[methodInstance.FullName].ToString());

	errorVariables.Add(errorLiteral.GetAncOfType(typeof(IndexerRef)).GetAssignee());
	if(errorLiteral.Count == 0)
		unassignedErrors.Add(indexerRef);
		
	indexerRefsAlreadyProcessed.Add(duplicatedMethods);
}

//-----  All errorVariables were discovered ------------//


//Remove unused variables
CxList rightSides = All.FindByAssignmentSide(CxList.AssignmentSide.Right);
errorVariables *= rightSides.GetAssignee();


//Remove member access
errorVariables -= errorVariables.FindByType(typeof(MemberAccess));


//Hack: Remove multiple variables declarations in same method 
//because currently the resolver is not returning the correct definition 
CxList variablesToRemove = All.NewCxList();
CxList errorVariablesDeclarators = errorVariables.FindByType(typeof(Declarator));
CxList variablesAlreadyProcessed = All.NewCxList();
foreach(CxList v in errorVariablesDeclarators)
{
	if((variablesAlreadyProcessed * v).Count > 0)
		continue;
	
	CxList list = errorVariablesDeclarators.FindByShortName(v.GetName());
	CxList methodDecls = list.GetAncOfType(typeof(MethodDecl));
	
	variablesAlreadyProcessed.Add(list);
	foreach (CxList m in methodDecls)
	{
		CxList variableDeclInM = list.GetByAncs(m);
		if(variableDeclInM.Count > 1)
		{	
			variablesToRemove.Add(list);
			list -= variableDeclInM;
		}
	}
}
errorVariables -= variablesToRemove;

// Remove returned error variables
CxList errorReferences = All.FindAllReferences(errorVariables);
CxList errorReferencesInReturn = errorReferences.GetByAncs(returnStmts);
errorReferences -= errorReferencesInReturn;


CxList errorVariablesHandled = All.NewCxList();
errorVariablesHandled.Add(errorVariables.GetByAncs(Find_FieldDecls()));
errorVariablesHandled.Add(errorVariables.FindDefinition(errorReferencesInReturn));
errorVariablesHandled.Add(errorVariables.FindAllReferences(errorReferencesInReturn));
errorVariablesHandled.Add(errorReferences.DataInfluencingOn(errorHandlers).GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly));

errorVariables = errorVariables - errorVariablesHandled;


result.Add(unassignedErrors);
result.Add(errorVariables);