CxList methodDecls = Find_MethodDecls();
CxList testingTypes = All.FindByType("Testing.*");
testingTypes.Add(All.FindByType("check.*"));

result = methodDecls.FindByParameters(testingTypes);