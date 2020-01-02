CxList unknownRefs = Find_Unknown_References();
CxList memoryAllocation = Find_Memory_Allocation();
CxList memoryDeallocation = Find_Memory_Deallocation();
CxList throwStmts = Find_ThrowStmt();
CxList destructorDecls = Find_DestructorDecl();
CxList decls = Find_VariableDeclStmt();
decls.Add(Find_FieldDecls());
CxList declarators = Find_Declarators();

// 1. Throw statement between memory allocations and memory deallocation
CxList throwAfterAllocation = All.NewCxList();
foreach(CxList allocation in memoryAllocation)
{
	CxList foundThrow = throwStmts.FindInScope(allocation, memoryDeallocation);
	if(foundThrow.Count == 1)
	{
		throwAfterAllocation.Add(allocation.ConcatenatePath(foundThrow, false));
	}
}

// 2. Remove allocations that influence on a memory deallocation
memoryAllocation -= memoryAllocation.DataInfluencingOn(memoryDeallocation)
	.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);

// 3. Find allocated declaration and its references
CxList allocatedReferences = memoryAllocation.GetAssignee();
CxList unaryReferences = allocatedReferences.FindByType(typeof(UnaryExpr));
allocatedReferences.Add(unknownRefs.GetByAncs(unaryReferences));
allocatedReferences.Add(memoryAllocation.GetFathers().FindByType(typeof(CastExpr)).GetAssignee());
allocatedReferences.Add(memoryAllocation * unknownRefs);

CxList assignedAllocation = allocatedReferences.GetAssigner() * memoryAllocation;
assignedAllocation.Add(memoryAllocation.FindByFathers(
	allocatedReferences.GetAssigner().FindByType(typeof(CastExpr))));
memoryAllocation -= assignedAllocation;

// 4. Free inside class destructor
CxList allAllocatedReferences = unknownRefs.FindAllReferences(allocatedReferences);
CxList deallocatedInDestructor = allAllocatedReferences.GetParameters(memoryDeallocation.GetByAncs(destructorDecls));
allocatedReferences -= allAllocatedReferences.FindAllReferences(deallocatedInDestructor);

// 5. Allocated references that influence on a memory deallocation (whose allocation don't produces flow...)
allocatedReferences -= unaryReferences;
allocatedReferences -= allocatedReferences.InfluencingOn(memoryDeallocation);

// 6. Disregard allocation to static variables (not static pointers)
declarators -= Find_Pointers();
CxList staticVarDeclarators = declarators.GetByAncs(decls.FindByFieldAttributes(Modifiers.Static));
CxList staticVarRefs = unknownRefs.FindAllReferences(staticVarDeclarators);
allocatedReferences -= staticVarRefs;

result = allocatedReferences;
result.Add(memoryAllocation);
result.Add(throwAfterAllocation);