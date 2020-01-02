CxList methods = Find_Methods();
CxList sqlitedb = methods.FindByMemberAccess("SqLiteDataBase.*");
CxList db = sqlitedb.FindByMemberAccess("SqLiteDataBase.delete"); 
db.Add(sqlitedb.FindByMemberAccess("SqLiteDataBase.insert*"));
db.Add(sqlitedb.FindByMemberAccess("SqLiteDataBase.query*"));

CxList rawQuery = sqlitedb.FindByMemberAccess("SqLiteDataBase.rawQuery");
CxList rawQueryWithFactory = sqlitedb.FindByMemberAccess("SqLiteDataBase.rawQueryWithFactory");
CxList execSQL = sqlitedb.FindByMemberAccess("SqLiteDataBase.execSQL");

result = All.GetParameters(db);
result.Add(All.GetParameters(rawQuery, 1)); // in rawQuery only parameter 0 is vulnerable to injection
result.Add(All.GetParameters(rawQuery, 2));
result.Add(All.GetParameters(rawQueryWithFactory, 0)); // in rawQueryWithFactory only parameter 1 is vulnerable to injection
result.Add(All.GetParameters(rawQueryWithFactory, 2));
result.Add(All.GetParameters(rawQueryWithFactory, 3));
result.Add(All.GetParameters(execSQL, 1)); // in execSQL only parameter 0 is vulnerable to injection