CxList sqliteLib = Find_Require("sqlite");
CxList sqliteLibDB = sqliteLib.GetMembersOfTarget();
sqliteLibDB.Add(All.FindAllReferences(sqliteLib.GetAssignee()).GetMembersOfTarget());
sqliteLibDB = sqliteLibDB.FindByShortName("open");
CxList sqliteLibDBRefs = sqliteLibDB.GetAssignee();
CxList sqliteLibPromiseThen = All.FindAllReferences(sqliteLibDBRefs).GetMembersOfTarget().FindByShortName("then");
CxList sqliteLibPromiseHandler = Get_MethodDecl_of_UnknownRef(All.GetParameters(sqliteLibPromiseThen));
CxList sqliteLibDatabasesRefs = All.FindAllReferences(All.GetParameters(sqliteLibPromiseHandler, 0));
sqliteLibDatabasesRefs.Add(All.FindAllReferences(sqliteLibDatabasesRefs.GetAssignee()));

result.Add(All.FindAllReferences(sqliteLibDBRefs));
result.Add(sqliteLibDatabasesRefs);