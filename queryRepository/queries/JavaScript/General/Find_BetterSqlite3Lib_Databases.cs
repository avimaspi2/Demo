CxList instances = Find_ObjectCreations();

CxList betterSqliteLib = Find_Require("better-sqlite3");
CxList betterSqlite3Refs = Find_TypeRef().FindAllReferences(betterSqliteLib);
CxList betterSqlite3DatabaseCreation = betterSqlite3Refs.GetFathers() * instances;
CxList betterSqlite3Databases = All.FindAllReferences(betterSqlite3DatabaseCreation.GetAssignee());

result.Add(betterSqlite3Databases);