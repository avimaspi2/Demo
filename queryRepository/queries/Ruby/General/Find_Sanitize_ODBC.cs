CxList allMethods = Find_Methods();

CxList executeCalls = allMethods.FindByShortName("odbc_execute");

CxList executeParam2 = All.GetParameters(executeCalls, 1);

result.Add(executeParam2);