CxList sanitizers = All.NewCxList();
CxList methods = Find_Methods();
CxList memoryDeallocationMethods = Find_Memory_Deallocation();

CxList allParameters = Find_Parameters();
allParameters.Add(Find_Arrays()); 
CxList searchList = All - allParameters;
CxList firstParam = searchList.GetParameters(memoryDeallocationMethods);
//remove null pointers
firstParam -= firstParam.FindByAbstractValue(nullAbsValue => nullAbsValue is NullAbstractValue);
//remove dead code
firstParam -= firstParam.GetByAncs(Find_Dead_Blocks_From_Conditions());

//Get member where param.member();
CxList members = firstParam.GetTargetOfMembers().GetMembersOfTarget();
firstParam.Add(members);

//remove param where param.member()
CxList targetOfMembers = members.GetTargetOfMembers();
firstParam -= targetOfMembers;

CxList memoryAllocationMethods = Find_Memory_Allocation();
sanitizers.Add(memoryAllocationMethods);
CxList memoryAllocationMethodsParam = searchList.GetParameters(memoryAllocationMethods, 0);

CxList flows = firstParam.InfluencedByAndNotSanitized(firstParam, sanitizers);
flows.Add(firstParam.InfluencingOn(memoryAllocationMethodsParam));

/* 
   Clear fp's where free(pointer->prop) free(pointer)
   This results appear due because prop will influence pointer of second free
   pointer can't be added to sanitizers because will cause FN's in case of
   free(pointer->prop) free(pointer->prop)
*/
CxList toRemove = All.NewCxList();
CxList types = Find_TypeRef();

CxList exitMethod = methods.FindByShortName("exit");
CxList statementColWithBreaks = All.FindByType(typeof(BreakStmt)).GetAncOfType(typeof(StatementCollection));

foreach(CxList flow in flows.GetCxListByPath()){
	CxList startNode = flow.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
	CxList endNode = flow.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
	
	CxList startNodeDefinition = All.FindDefinition(startNode);
	CxList endNodeDefinition = All.FindDefinition(endNode);
	
	CxList startNodePossibleDefinitions = startNodeDefinition.GetAncOfType(typeof(VariableDeclStmt));
	startNodePossibleDefinitions.Add(startNodeDefinition.GetAncOfType(typeof(FieldDecl)));
	startNodePossibleDefinitions.Add(startNodeDefinition.GetAncOfType(typeof(ParamDecl)));
	CxList startNodeType = types.GetByAncs(startNodePossibleDefinitions);
	
	CxList endNodePossibleDefinitions = endNodeDefinition.GetAncOfType(typeof(VariableDeclStmt));
	endNodePossibleDefinitions.Add(endNodeDefinition.GetAncOfType(typeof(FieldDecl)));
	endNodePossibleDefinitions.Add(endNodeDefinition.GetAncOfType(typeof(ParamDecl)));
	CxList endNodeType = types.GetByAncs(endNodePossibleDefinitions);
	
	TypeRef startNodeTypeRef = startNodeType.TryGetCSharpGraph<TypeRef>();
	TypeRef endNodeTypeRef = endNodeType.TryGetCSharpGraph<TypeRef>();
	
	//types are different
	if(startNodeTypeRef != null && endNodeTypeRef != null
	&& startNodeTypeRef.TypeName != endNodeTypeRef.TypeName){
		toRemove.Add(flow);	
	}
		//we don't know the definition
	else if(startNodeTypeRef == null || endNodeTypeRef == null){
		toRemove.Add(flow);	
	}
		//check for flow breakers
		//exit
	else{
		CxList startNodeScope = startNode.GetAncOfType(typeof(StatementCollection));
		CxList exitMethodScope = exitMethod.GetAncOfType(typeof(StatementCollection));
		CxList endNodeScope = endNode.GetAncOfType(typeof(StatementCollection));
		CxList scope = startNodeScope * exitMethodScope * endNodeScope;
		//Are the three relevant Nodes in same scope?
		if(scope.Count == 1){
			//is the exit beteween the first free and last free?
			if(exitMethod.FindInScope(startNode, endNode).Count > 0){
				toRemove.Add(flow);
				continue;
			}
				//is exit before the first free?
			else if(exitMethod.FindInScope(scope, startNode).Count > 0){
				toRemove.Add(flow);
				continue;
			}
		}
		
		CxList isBreakInSameScopeAsStartNode = startNodeScope * statementColWithBreaks;
		CxList ParentCase = isBreakInSameScopeAsStartNode.GetAncOfType(typeof(Case));
		CxList ParentCaseOfEndNode = endNode.GetAncOfType(typeof(Case));
		CxList haveSameParentCase = ParentCase * ParentCaseOfEndNode;
		if(haveSameParentCase.Count == 1){
			toRemove.Add(flow);
		}
	}
}

flows -= toRemove;

/* 
	Try to find memory deallocation using Abstract interpretation
*/
CxList releasedPointers = All
	.FindByAbstractValue(_ => 
	{
	if (_ is ObjectAbstractValue){
		ObjectAbstractValue obj = (ObjectAbstractValue) _;
		return obj.AllocationState == ObjectAllocationState.Released;
	}
	return false;
	});

CxList doubleFreePointers = releasedPointers.InfluencingOn(memoryDeallocationMethods);


/* 
	Remove duplicated flows
	Explanation: the absint flows are slightly different and the reduce flow can't relate them. The solution
		basically removes those that have the same start node that is the end node of the other flow.
*/
CxList duplicatedDoubleFreePointers = All.NewCxList();
foreach (CxList doubleFreePointerFlow in doubleFreePointers.GetCxListByPath())
{
	CxList firstDoubleFreeNode = doubleFreePointerFlow.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
	foreach (CxList rFlow in flows.GetCxListByPath())
	{
		CxList lastNodeOfRFlow = rFlow.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
		if (lastNodeOfRFlow == firstDoubleFreeNode)
			duplicatedDoubleFreePointers.Add(doubleFreePointerFlow);
	}				
}
CxList doubleFreeAbsintFlows = doubleFreePointers - duplicatedDoubleFreePointers;

// Remove object indexes being freed
CxList correctFlows = All.NewCxList();

foreach (CxList potentialFlows in flows.GetCxListByPath()) 
{
	CxList allNodes = potentialFlows.GetStartAndEndNodes(CxList.GetStartEndNodesType.AllNodes);
	CxList sNode = potentialFlows.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
	
	CxList sNodeDef = All.FindDefinition(sNode);
	CxList sNodeRef = All.FindAllReferences(sNodeDef);
	
	CxList sNodeRefsInsideFlow = sNodeRef * allNodes;
	CxList sNodeRefsInAssignes = sNodeRefsInsideFlow.GetByAncs(Find_AssignExpr());
	
	CxList nodeWithIndexers = allNodes.FindByFathers(Find_IndexerRefs());
	
	CxList assignFromStartNode = sNodeRefsInAssignes.GetAncOfType(typeof(AssignExpr));	
	CxList assignFromIndexers = nodeWithIndexers.GetAncOfType(typeof(AssignExpr));
	
	int id1 = -1;
	int id2 = -2;
	
	AssignExpr n1 = assignFromStartNode.TryGetCSharpGraph<AssignExpr>();
	if (n1!=null)
		id1 = n1.NodeId;
	
	AssignExpr n2 = assignFromIndexers.TryGetCSharpGraph<AssignExpr>();
	if (n2!=null)
		id2 = n2.NodeId;
	
	if ( id1 != id2)
	{
		correctFlows.Add(potentialFlows);
	}
}

/*
	Prepare the results
*/
result.Add(correctFlows);
result.Add(doubleFreeAbsintFlows);
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);