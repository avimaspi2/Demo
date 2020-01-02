CxList Databases = Find_QSqlite3Lib_Databases();
List<string> stMethods = new List<string>{"prepare"};
CxList qSqliteLibDatabase = Databases.GetMembersOfTarget().FindByShortNames(stMethods);
CxList promiseCallBack = Get_MethodDecl_of_UnknownRef(Get_Promise_Results(qSqliteLibDatabase));
CxList qsqLite3StatementReferences = All.FindAllReferences(All.GetParameters(promiseCallBack, 0));
qsqLite3StatementReferences.Add(All.FindAllReferences(qsqLite3StatementReferences.GetAssignee()));
result = qsqLite3StatementReferences;