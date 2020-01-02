/*
	Memory Allocation that is automatically freed
*/

CxList methods = Find_Methods();

// Find memory allocation methods
List<string> allocationMethods = new List<string> {"alloca", "_alloca"};
CxList memoryAllocationMethods = methods.FindByShortNames(allocationMethods);
result.Add(memoryAllocationMethods);