CxList methods = Find_DB_In_Peewee();
methods.Add(Find_DB_Out_Peewee());

CxList allParams = Find_Param();


CxList execute = methods.FindByName("*.execute_query");
//only the second parameter of execute_query is sanitized
result.Add(allParams.GetParameters(execute, 1));
CxList allMethods = Find_Methods();
CxList rawquery = allMethods.FindByShortName("RawQuery");
rawquery.Add(Find_ObjectCreations().InheritsFrom("RawQuery"));
rawquery.Add(allMethods.FindByShortName("raw"));

//only the third parameter of RawQuery is sanitized
result.Add(allParams.GetParameters(rawquery, 1));