string[] sanitizerList = new string[]{"sync.waitgroup", "sync.mutex", "chan"};
string[] sanitizedVars = new string[]{"put", "get", "lock", "unlock", "wait", "add", "done"};
string[] sanitizedInvokes = new string[]{"\"strings\"", "\"sync/atomic\""};

CxList allVars = Find_UnknownReferences();

//Find all goroutines
CxList parallelMethods = All.FindByCustomAttribute("goroutine").GetAncOfType(typeof(MethodInvokeExpr));

//Get all the sanitizers (exclude pointers)
CxList sanitizers = All.FindByTypes(sanitizerList);
sanitizers -= sanitizers.FindByType("Pointer");
sanitizers = All.FindAllReferences(sanitizers);

//Exclude sanitized methods
parallelMethods = All.FindDefinition(parallelMethods);
parallelMethods -= parallelMethods.GetMethod(sanitizers);
parallelMethods -= parallelMethods.GetByAncs(sanitizers.GetAncOfType(typeof(LambdaExpr)));

//Get vars that are used inside unsanitized goroutines (exludes the ones declared inside)
CxList vars = allVars.GetByAncs(parallelMethods);

CxList varsToExclude = Find_ParamDecl().GetByAncs(parallelMethods);
varsToExclude -= varsToExclude.FindByType("Pointer");
varsToExclude -= vars.FindByShortName("this").GetByAncs(parallelMethods);

varsToExclude.Add(Find_Declarators().GetByAncs(parallelMethods));
varsToExclude.Add(vars.FindByTypes(sanitizedInvokes));
varsToExclude = vars.FindAllReferences(varsToExclude);

CxList rightMembers = vars.GetRightmostMember().FindByType(typeof(MethodInvokeExpr)).FindByTypes(sanitizedVars);
varsToExclude.Add(rightMembers.GetLeftmostTarget());

vars -= varsToExclude;

result = vars;