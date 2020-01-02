/*
	Memory Allocation
*/

CxList methods = Find_Methods();
CxList unknownReferences = Find_Unknown_References();
CxList objectCreations = Find_ObjectCreations();
CxList arrayCreations = Find_ArrayCreateExpr();
CxList unaryExpressions = Find_Unarys();

// Find memory allocation methods
List<string> allocationMethods = new List<string> {
		"malloc", "realloc", "calloc", "aligned_alloc", "strdup",
		"HeapAlloc", "LocalAlloc", "GlobalAlloc", "_strdup", "_wcsdup",
		"_mbsdup", "xmalloc", "xrealloc", "xcalloc", "xstrdup", "opendir",
		"fdopendir", "backtrace_symbols", "valloc", "memalign", "pvalloc" };
CxList memoryAllocationMethods = methods.FindByShortNames(allocationMethods);

// Find memory allocation for objects
CxList objectAllocations = All.FindByCustomAttribute("new")
	.GetAncOfType(typeof(ObjectCreateExpr));
	
// Find memory allocation for arrays
CxList arrayDeclarations = arrayCreations.GetAssignee();
objectAllocations.Add(arrayDeclarations.FindByPointerType("*").GetAssigner());
CxList unaryPointer = unaryExpressions.FindByShortName("Pointer");
objectAllocations.Add((arrayDeclarations * unaryPointer).GetAssigner());

// Find memory allocated parameters
List <string> parameterAllocationMethods = new List<string> {
		"asprintf", "vasprintf", "posix_memalign" };
CxList methodsWithParameterAllocation = methods.FindByShortNames(parameterAllocationMethods);
CxList allocatedParams = All.GetParameters(methodsWithParameterAllocation, 0);
CxList allocatedParamRef = unknownReferences.GetByAncs(allocatedParams);

result.Add(memoryAllocationMethods);
result.Add(objectAllocations);
result.Add(allocatedParamRef);