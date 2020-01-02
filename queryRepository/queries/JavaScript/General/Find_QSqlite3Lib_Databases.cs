CxList qSqlite3Lib = Find_Require("q-sqlite3");
CxList qSqliteLibDatabase = qSqlite3Lib.GetMembersOfTarget().FindByShortName("createDatabase");
CxList promiseCallBack = Get_MethodDecl_of_UnknownRef(Get_Promise_Results(qSqliteLibDatabase));
CxList qsqLite3DatabaseReferences = All.FindAllReferences(All.GetParameters(promiseCallBack, 0));
qsqLite3DatabaseReferences.Add(All.FindAllReferences(qsqLite3DatabaseReferences.GetAssignee()));

result.Add(qsqLite3DatabaseReferences);