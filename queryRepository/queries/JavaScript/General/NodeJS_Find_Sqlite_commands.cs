//Gets all the methods executed on a Sqlite3 DB
CxList references = Find_UnknownReference();
references.Add(Find_MemberAccesses());

CxList sqLite3Databases = Find_Require("sqlite3");
CxList methods = Find_Methods().FindByShortName("require");
CxList sqliteParam = Find_Parameters().FindByShortName("\"sqlite3\"");
CxList sqlite3Lib = methods.FindByParameters(sqliteParam);

CxList sqlite3Verbose = sqlite3Lib.GetMembersOfTarget().FindByShortName("verbose").GetAssignee();
sqlite3Lib.Add(references.FindAllReferences(sqlite3Verbose));
sqLite3Databases.Add(Get_Instance_Of_Required(sqlite3Lib, "Database"));
result.Add(sqLite3Databases.FindByType(typeof(ObjectCreateExpr)));

CxList sqLiteDatabasesRefs = references.FindAllReferences(sqLite3Databases);
result.Add(sqLiteDatabasesRefs.GetMembersOfTarget());