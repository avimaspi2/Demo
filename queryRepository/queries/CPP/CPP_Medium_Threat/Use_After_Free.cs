CxList deallocations = Find_Memory_Deallocation();
CxList allocations = Find_Memory_Allocation();
CxList unknownReferences = Find_Unknown_References();
CxList methodInvocations = Find_Methods();
CxList memberAccesses = Find_MemberAccesses();
CxList indexerReferences = Find_IndexerRefs();
CxList binaryExpressions = Find_BinaryExpressions();
CxList unaryExpressions = Find_Unarys();
CxList origin = (unknownReferences + memberAccesses);

/*
    Define usages
 */

// Find safe print methods that only use %p specifier
CxList formatParameterLiterals = Get_Format_Parameter().FindByType(typeof(StringLiteral));
Regex unsafeSpecifiersRegex = new Regex(@"%[-+ #0]*(\d+|\*)?(\.\d+|\.\*)?(hh|h|ll|l|j|z|t|L)?[diuoxXfFeEgGaAcsn]");
CxList safeMethodInvocations = All.NewCxList();
foreach(CxList formatP in formatParameterLiterals)
{
	StringLiteral l = formatP.TryGetCSharpGraph<StringLiteral>();
	String literalContent = l.FullName;
	// If literal content does not match unsafe regex, it is a safe specifier case
	if (!unsafeSpecifiersRegex.IsMatch(literalContent)) 
	{
		safeMethodInvocations.Add(formatP);
	}
}
safeMethodInvocations = safeMethodInvocations.GetAncOfType(typeof(MethodInvokeExpr));

// Find method usages: origin parameters and origin elements in indexer parameters of vulnerable methods
CxList vulnerableMethods = methodInvocations - safeMethodInvocations;
CxList methodUsages = origin.GetParameters(vulnerableMethods);

// Find binary usages: remove nested expressions and use remaining (outter) expressions 
CxList nestedBinaryExpressions = binaryExpressions.FindByFathers(unaryExpressions + binaryExpressions);
CxList binaryExpressionsUsages = binaryExpressions - nestedBinaryExpressions;

// Find unary usages: remove nested expressions and find origin elements in remaining expressions
CxList nestedUnaryExpressions = unaryExpressions.FindByFathers(unaryExpressions + binaryExpressions);
CxList unaryExpressionsUsages = origin.GetByAncs(unaryExpressions - nestedUnaryExpressions);

// Find accessing address of an array by index
CxList safeArrayAccess = indexerReferences.GetParameters(safeMethodInvocations);
CxList arrayAccessUsage = unknownReferences.GetByAncs(indexerReferences - safeArrayAccess);

CxList usages = methodUsages;
usages.Add(unaryExpressionsUsages);
usages.Add(binaryExpressionsUsages);
usages.Add(origin.FindByAssignmentSide(CxList.AssignmentSide.Right));
usages.Add(arrayAccessUsage);
usages -= allocations;

/*
    Find use after free flows
 */

// Find flows from deallocation parameters (references or members) to usages
CxList parameters = origin.GetParameters(deallocations);
CxList flows = parameters.DataInfluencingOn(usages);
flows = flows.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);

// Find points that may break flows
CxList flowBreakingStmts = All.FindByType(typeof(BreakStmt));
flowBreakingStmts.Add(All.FindByType(typeof(ThrowStmt)));
flowBreakingStmts.Add(All.FindByType(typeof(ContinueStmt)));
flowBreakingStmts.Add(methodInvocations.FindByShortName("THROW_LAST", true));
flowBreakingStmts.Add(methodInvocations.FindByShortName("THROW", true));

CxList types = Find_TypeRef();
CxList methods = Find_Methods();
CxList exitMethod = methods.FindByShortName("exit");

result = flows;

CxList pointExpressions = unaryExpressions.FindByShortName("Pointer");

CxList toRemove = All.NewCxList();
foreach(CxList flow in flows.GetCxListByPath())
{
	if (flow.Count == 0)
	{
		continue;
	}
	CxList startNode = flow.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
	CxList endNode = flow.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
	
	// Find cases where the endNode is the pointer dereference of the startNode: free(x); *x=1;
	if ( (endNode * pointExpressions).Count > 0)
	{
		endNode.Add(unknownReferences.GetByAncs(endNode));
	}
	
	// Find cases where the endNode is a reference of the startNode: delete x; x->y = 1;
	CxList reference = startNode.FindAllReferences(endNode);
	if (reference.Count > 0)
	{
		continue;
	}
	
	// Find cases where we use a target of a freed member: free(x->y); doSomething(x);
	CxList targetOfFreedMember = startNode.GetTargetOfMembers();
	bool isReferenceOfUsage = targetOfFreedMember.FindAllReferences(endNode).Count > 0;
	if (isReferenceOfUsage)
	{
		toRemove.Add(flow);
		continue;
	}
	
	CSharpGraph startGraph = startNode.TryGetCSharpGraph<CSharpGraph>();
	TypeRef startNodeTypeRef = null;
	if (startGraph is UnknownReference) startNodeTypeRef = ((UnknownReference) startGraph).DeclaratorType; 
	if (startGraph is MemberAccess) startNodeTypeRef = ((MemberAccess) startGraph).DeclaratorType;				

	CSharpGraph endGraph = endNode.TryGetCSharpGraph<CSharpGraph>();
	TypeRef endNodeTypeRef = null;
	if (endGraph is UnknownReference) endNodeTypeRef = ((UnknownReference) endGraph).DeclaratorType; 
	if (endGraph is MemberAccess) endNodeTypeRef = ((MemberAccess) endGraph).DeclaratorType;				

	//we don't know the definition
	if (startNodeTypeRef == null || endNodeTypeRef == null)	{
		toRemove.Add(flow);
		continue;
	}
	//types are different
	if(startNodeTypeRef.TypeName != endNodeTypeRef.TypeName){
		toRemove.Add(flow);
		continue;
	}
	
	//check for flow breakers
	CxList enclosingStatementCollection = (startNode + exitMethod + endNode).GetAncOfType(typeof(StatementCollection));
	if(enclosingStatementCollection.Count == 1 && (exitMethod.FindInScope(startNode, endNode).Count > 0)){
		toRemove.Add(flow);
		continue;
	}
	CxList breakInsideFlow = flowBreakingStmts.FindInScope(startNode, endNode);
	if (breakInsideFlow.Count > 0 ) {
		toRemove.Add(flow);
	}
}
result -= toRemove;

// Find flows from pointer parameters to other pointer expressions
CxList unaryPointParams = pointExpressions.GetParameters(deallocations);
CxList unaryPointFlows = (origin.GetByAncs(unaryPointParams)).DataInfluencingOn(origin.GetByAncs(pointExpressions));
result.Add(unaryPointFlows);

// Find flows from indexer parameters to origin elements in references of said parameters - only those dealloced outside loops...
CxList indexerParams = indexerReferences.GetParameters(deallocations);
CxList indexerParamsInLoop = indexerParams.GetByAncs(indexerParams.GetAncOfType(typeof(IterationStmt)));
indexerParams -= indexerParamsInLoop;
CxList refOfIndexerParams = indexerReferences.FindAllReferences(indexerParams);
CxList indexerFlows = (origin.GetByAncs(indexerParams)).InfluencingOn(origin.GetByAncs(refOfIndexerParams));
result.Add(indexerFlows);