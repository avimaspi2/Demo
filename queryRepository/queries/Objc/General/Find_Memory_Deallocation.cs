// Memory Deallocation

// Find all method calls
CxList methods = Find_Methods();

// Find Memory Deallocation
CxList dealloc = methods.FindByShortName("free"); 
dealloc.Add(methods.FindByShortName("delete"));

// Add all methods that end with deallocation ot the list of deallocation memory methods
CxList ParamDecl = Find_ParamDecl();
CxList deallocMethod = dealloc.GetAncOfType(typeof(MethodDecl));
CxList deallocatedParam = ParamDecl.GetByAncs(deallocMethod).FindAllReferences(All.GetByAncs(dealloc));
CxList methodsWithLessThan2Params = methods - methods.FindByParameters(All.GetParameters(methods, 1));
CxList deallocMethods = methodsWithLessThan2Params.FindAllReferences(deallocatedParam.GetAncOfType(typeof(MethodDecl)));

result = dealloc;
result.Add(deallocMethods);