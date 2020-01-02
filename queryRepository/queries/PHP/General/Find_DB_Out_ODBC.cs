CxList methods = Find_Methods();

// 1 - Explicite DB function names
CxList directDbMethods = methods.FindByShortNames(new List<string> 
	{"odbc_fetch_array", "odbc_fetch_object", "odbc_foreignkeys", "odbc_result"});

result.Add(directDbMethods);

// identify methods which one of their parameters are used for output.
// Then identify the paramenters themselves and then ...
CxList variableUpdatedDbMethod = 
	methods.FindByShortName("odbc_fetch_into");

CxList vudmParam = All.GetParameters(variableUpdatedDbMethod, 1);
result.Add(vudmParam);