CxList calls = Find_CALL_Statements();
CxList references = base.Find_UnknownReference();
CxList calledModule = references.GetParameters(calls, 0);

CxList undefinedModules = All.NewCxList();
	
foreach(CxList module in calledModule){
	if(All.FindDefinition(module).Count == 0)
		undefinedModules.Add(module);
}

result = undefinedModules;