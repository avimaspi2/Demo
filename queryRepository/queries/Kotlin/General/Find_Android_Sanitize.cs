CxList methods = Find_Methods();
CxList sqlitedb = methods.FindByMemberAccess("SqLiteDataBase.*");

CxList db = All.NewCxList();
db.Add(sqlitedb.FindByShortNames(new List<string> {"delete","insert*","query*"}));

CxList rawQuery = sqlitedb.FindByShortName("rawQuery");
CxList rawQueryWithFactory = sqlitedb.FindByShortName("rawQueryWithFactory");
CxList execSQL = sqlitedb.FindByShortName("execSQL");

result = All.GetParameters(db);

//in rawQuery only parameter 0 is vulnerable to injection
CxList allParameterRawQuery = All.GetParameters(rawQuery);
CxList firstParameterRawQuery = All.GetParameters(rawQuery,0);
result.Add(allParameterRawQuery - firstParameterRawQuery);
// in rawQueryWithFactory only parameter 1 is vulnerable to injection
CxList allParameterRawQueryWithFactory = All.GetParameters(rawQueryWithFactory);
CxList secondParameterRawQueryWithFactory = All.GetParameters(rawQueryWithFactory, 1);
result.Add(allParameterRawQueryWithFactory - secondParameterRawQueryWithFactory);
// in execSQL only parameter 0 is vulnerable to injection
CxList allParameterExecSql = All.GetParameters(execSQL);
CxList firstParameterExecSql = All.GetParameters(execSQL, 0);
result.Add(allParameterExecSql - firstParameterExecSql);