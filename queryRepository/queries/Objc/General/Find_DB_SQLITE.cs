CxList methods = Find_Methods();

List<string> allMethods = new List<string> {
		"sqlite3_exec",
		"sqlite3_prepare*",
		"sqlite3_get_table"	
		};

CxList dbMethods = methods.FindByShortNames(allMethods);

CxList executeQuery = methods.FindByShortName("executeQuery*");

result = All.GetParameters(dbMethods, 1);
result.Add(All.GetParameters(executeQuery, 0));

dbMethods = methods.FindByShortName("sqlite3_bind_*");
result.Add(All.GetParameters(dbMethods, 2));