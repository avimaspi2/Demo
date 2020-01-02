/*
In this query we look for variables that are allocated and never deallocated.
There are 2 scenarios that we deal with:
1. Memory allocation with no deallocation at all.
2. Memory allocation with a deallocation statement, but a return statement that appears before or in parallel
   to the deallocation.

We do not deal with more complex cases, such as closing in another method. We assume that
the dealloc should be in the same method.
*/

// General assignments, needed for the query
CxList UnknownReference = Find_UnknownReference();
CxList ReturnStmt = Find_ReturnStmt();
CxList ParamDecl = Find_ParamDecl();
CxList Param = Find_Param();
CxList ThisRef = Find_ThisRef();
CxList AssignExpr = Find_AssignExpr();
CxList Declarator = Find_Declarators();

CxList memAllocations = Find_Memory_Allocation();//.FindByShortName("realloc").FindByFileName("*good*");
CxList dealloc = Find_Memory_Deallocation();

// Get all under return statements and parameters.
CxList inReturnStmt = All.GetByAncs(ReturnStmt);
CxList inParam = All.GetByAncs(Param);
CxList inParamDecl = All.GetByAncs(ParamDecl);
inParamDecl -= inParamDecl.FindByType(typeof(TypeRef));

// All the x's in the case of "x = y;"
CxList unknownRefLeftSide = UnknownReference.GetByAncs(All.FindByAssignmentSide(CxList.AssignmentSide.Left));

// Find allocated variables that are on the left assignment side side of "x = malloc(..)"
CxList allAllocatedVariables = unknownRefLeftSide.GetByAncs(memAllocations.GetAncOfType(typeof(AssignExpr)));

// Add objects that were allocated at their declaration: "int *x = malloc(..)"
allAllocatedVariables.Add(memAllocations.GetAncOfType(typeof(Declarator)));

// See if there are any deallocations for every allocation parameter
CxList deallocParams = All.GetParameters(dealloc);
CxList deallocated = deallocParams.FindAllReferences(allAllocatedVariables);


/// 1. Memory allocation with no deallocation at all

// Find all methods that if the parameter is passed to them, it's not equivalent to deallocation
CxList irrelevantMethods = Find_All_strncat(); 
irrelevantMethods.Add(Find_All_strncpy());
irrelevantMethods.Add(Find_All_snprintf());
irrelevantMethods.Add(Find_All_Strlen()); 
irrelevantMethods.Add(Find_BO_Funcs());
irrelevantMethods.Add(Find_Bounded_Methods());
irrelevantMethods.Add(Find_Methods_With_Return_Value());

inParam -= inParam.GetParameters(irrelevantMethods);
// Remove all allocations that either have NO deallocation in the program, or the allocated variable is 
// a return value or a parameter of the method

CxList inReturnStmtParam = All.NewCxList();
inReturnStmtParam.Add(inReturnStmt);
inReturnStmtParam.Add(inParam);

CxList onlyAllocatedVariables = All.NewCxList();
onlyAllocatedVariables.Add(allAllocatedVariables);	
onlyAllocatedVariables -= allAllocatedVariables.FindAllReferences(deallocated);
onlyAllocatedVariables -= allAllocatedVariables.FindAllReferences(inReturnStmtParam);	

// Between all the variables that were never deallocated, leave only results that are not assigned to a parameter.
// This parameter is most likely passed by ref.
CxList inParamRefs = All.FindAllReferences(inParamDecl);
CxList assignOfUnknown = (unknownRefLeftSide - onlyAllocatedVariables).GetAncOfType(typeof(AssignExpr));
CxList allocatedVariablesRef = All.FindAllReferences(onlyAllocatedVariables);

foreach (CxList onlyAllocatedVariable in onlyAllocatedVariables)
{	// Pass on all variables that were allocated and not deallocated
	// Find all the assign expressions relevant to this specific allocated variable
	CxList assignOfAllocatedVariables = allocatedVariablesRef.FindAllReferences(onlyAllocatedVariable);
	assignOfAllocatedVariables = assignOfAllocatedVariables.FindByAssignmentSide(CxList.AssignmentSide.Right);
	assignOfAllocatedVariables = assignOfAllocatedVariables.GetByAncs(assignOfUnknown);
	// Add only allocated variables that were not assigned to anything
	if (assignOfAllocatedVariables.Count == 0)
	{
		result.Add(onlyAllocatedVariable);
	}
}


/// 2. Memory allocation with a deallocation statement, but a return statement that appears before or in parallel
///    to the deallocation

// Find if statements starting with "not" and containing null. Both are relevant to sanitize Memory Leak
CxList conditions = Find_Condition();
CxList negativeIf = conditions.FindByShortName("Not").GetFathers().FindByType(typeof(IfStmt));
CxList nullLiterals = Find_Null_Literals();
CxList nullIf = nullLiterals.GetByAncs(conditions).GetAncOfType(typeof(IfStmt));

CxList safeIfStatement = All.NewCxList();
safeIfStatement.Add(nullIf);
safeIfStatement.Add(negativeIf);

// Remove return statement the contain "this", because returning "this" sanitizing Memory Leak
CxList thisReturn = ThisRef.GetAncOfType(typeof(ReturnStmt));
ReturnStmt -= thisReturn;

// Performance optimization
CxList allocatedVariablesInCondition = All.FindAllReferences(allAllocatedVariables).GetByAncs(conditions);
CxList inAssign = All.GetByAncs(AssignExpr);
CxList methods = Find_Methods();

CxList methodParam = All.NewCxList();
methodParam.Add(methods);

CxList dellocIrrelevantMethods = All.NewCxList();
dellocIrrelevantMethods.Add(dealloc);
dellocIrrelevantMethods.Add(irrelevantMethods);

methodParam -= dellocIrrelevantMethods;


CxList allocationInParameters = All.GetParameters(methodParam);

// No need to again search on results that we already checked
memAllocations -= memAllocations.GetByAncs(onlyAllocatedVariables);	

// Look at all the memory allocations and return the ones that are not deallocated
foreach (CxList allocation in memAllocations)
{
	CxList method = allocation.GetAncOfType(typeof(MethodDecl));
	
	// Get the variables with memory allocations in an assign expression
	CxList allocationInAssign = inAssign.FindByFathers(allocation.GetAncOfType(typeof(AssignExpr))).FindByAssignmentSide(CxList.AssignmentSide.Left);
	// We need the following line in case of a pointer: "*out = malloc(...)"
	// where only the pointer's assignment side is left
	allocationInAssign = inAssign.GetByAncs(allocationInAssign);
	// Get the variables with memory allocation in a declaration
	CxList decl = allocation.GetAncOfType(typeof(Declarator));
	CxList allocatedVariables = All.NewCxList();
	allocatedVariables.Add(decl);
	allocatedVariables.Add(allocationInAssign);
	
	CxList declInMethod = Declarator.GetByAncs(method);
	if (declInMethod.FindDefinition(allocatedVariables).Count == 0)
	{	// We allocate memory to a variable not from this method, so most likely it's OK not to free it here
		continue;
	}

	// See if the allocated variable is sent to a method sometwhere in the method - good heuristic for false positive
	if (allocationInParameters.GetByAncs(method).FindAllReferences(allocatedVariables).Count > 0)
	{
		continue;
	}
	
	// See if the allocated variable is assigned to some other variable, if so - false alarm
	CxList leftParam = unknownRefLeftSide.GetByAncs(method).GetAncOfType(typeof(AssignExpr));
	CxList relevantParams = (allocatedVariables * inParamRefs).FindAllReferences(inParamDecl.GetByAncs(method)).GetByAncs(leftParam);
	if (allocation.GetByAncs(relevantParams.GetAncOfType(typeof(AssignExpr))).Count > 0) 
	{
		continue;
	}

	// Find if there is a return statement in the method, in order to check if we return withou deallocating the variable
	CxList returnInMethod = ReturnStmt.GetByAncs(method);
	
	// Add last statement of each method to the return statements list
	MethodDecl g = method.TryGetCSharpGraph<MethodDecl>();
	if (g != null)
	{
		StatementCollection stcl = g.Statements;
		if (stcl != null)
		{
			CxList l = All.NewCxList();
			int maxId = 0;
			foreach (Statement s in stcl)
			{
				int nodeId = s.NodeId;
				if (nodeId > maxId)
				{
					maxId = nodeId;
					l = All.FindById(maxId);
				}
			}
			returnInMethod.Add(l - thisReturn); // return of self/this was removed from the return statements list,
			// so we don't want to add them back now
		}
	}
	
	// Remove the return statements that contain the allocated variables
	CxList allocatedVariablesInReturn = inReturnStmt.GetByAncs(returnInMethod).FindAllReferences(allocatedVariables);
	returnInMethod -= allocatedVariablesInReturn.GetAncOfType(typeof(ReturnStmt));
	
	// Remove the if statements that contain the allocated variables
	CxList allocatedVariablesInIf = allocatedVariablesInCondition.FindAllReferences(allocatedVariables);
	CxList ifWithTarget = allocatedVariablesInIf.GetAncOfType(typeof(IfStmt));
	returnInMethod -= returnInMethod.GetByAncs(ifWithTarget * safeIfStatement);
	
	// All deallocated variables in deallocation functions
	CxList deallocatedVariables = deallocParams.FindAllReferences(allocatedVariables);

	// Pass on all the return statements that are not affected by allocated variables, and not if Statements
	// containing these variables, and see if we need to add them to the result as "returned and didn't deallocate"
	foreach (CxList r in returnInMethod)
	{
		// retStmt is filled with the statement collection of this return statement,
		// and the containing statement collection
		CxList retStmt = r.GetAncOfType(typeof(StatementCollection));
		retStmt.Add(retStmt.GetFathers().GetAncOfType(typeof(StatementCollection)));
		// See if we have a deallocation in this statement collection (or its predecessor)
		CxList correctDeallocation = deallocatedVariables.GetByAncs(retStmt);
		// leave only the dealloc statements that are in the same level as the return statement.
		// dealloc statements inside an interior statements are not needed
		correctDeallocation = correctDeallocation.GetByAncs(retStmt * correctDeallocation.GetAncOfType(typeof(StatementCollection)));
		// No deallocation was done in this block
		if (correctDeallocation.Count == 0)
		{
			CSharpGraph allocationG = allocation.TryGetCSharpGraph<CSharpGraph>();
			CSharpGraph rG = r.TryGetCSharpGraph<CSharpGraph>();
			try
			{
				// If the current allocation appears before the return statement - it's probably a problem
				if (allocationG.NodeId < rG.NodeId)
				{
					
					CxList allocationToReturn = allocation.DataInfluencingOn(All.FindByFathers(r));
					// if there is no influence between the allocation and return statement - add it to the list
					if (allocationToReturn.Count == 0)
					{
						result.Add(allocation.Concatenate(r));
					}
				}
			}
			catch (Exception ex)
			{
				cxLog.WriteDebugMessage(ex);
			}
		}
	}
}