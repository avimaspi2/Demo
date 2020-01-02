CxList start = All.NewCxList();
CxList declarators = Find_All_Declarators();
CxList methods = Find_Methods();
CxList methodDecls = Find_Method_Declarations();
CxList allocs = Find_Memory_Allocation();
CxList deallocations = Find_Memory_Deallocation();
CxList pointers = Find_Pointer_Decl(); 
CxList builtInTypes = Find_Builtin_Types();

// Declarators beeing allocated
CxList declaratorsAllocated = allocs.GetAncOfType(typeof(Declarator));

// Get only the valid starting points
start = declarators - declaratorsAllocated;
start -= pointers; 

// Remove built-in type vars (keeps arrays and strings)
CxList arraysAndStrings = Find_Array_Declaration();
arraysAndStrings.Add(Find_Strings());
foreach (CxList builtIn in builtInTypes)
{
	if (arraysAndStrings.GetByAncs(builtIn).Count == 0)
	{
		start -= builtIn;
	}
}

// All var's being freed
CxList freedVars = All.GetParameters(deallocations, 0);

// Realloc can cause the memory being freed
allocs -= methods.FindByShortName("realloc");

// All allocations and pointers are considered sanitizers
CxList sanitizers = All.NewCxList();
sanitizers.Add(allocs);

sanitizers.Add(methods.FindByShortName("sizeof"));
sanitizers.Add(Find_Integers().FindByFathers(Find_ReturnStmt()));
sanitizers.Add(Find_Integers() * methods);
sanitizers.Add(All.GetParameters(methods.FindByShortName("strchr"), 1));
sanitizers.Add(All.GetParameters(Find_ObjectCreations()));

// Sanitize flows that pass through methods without definition
foreach (CxList methodInvoke in methods)
{
	if (methodDecls.FindDefinition(methodInvoke).Count == 0)
	{
		sanitizers.Add(methodInvoke);
	}
}

CxList allocatedFreedVars = allocs.GetAssignee();
freedVars -= freedVars.FindAllReferences(allocatedFreedVars);

// Flow
CxList flowNotSanitized = freedVars.InfluencedByAndNotSanitized(start, sanitizers)
	.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);

result = flowNotSanitized;