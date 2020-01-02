CxList dbs = Find_Sqlite3Lib_Databases();
var dbMembers = new List<string>{"prepare"};
CxList StatementMethods = dbs.GetMembersOfTarget().FindByShortNames(dbMembers);

result.Add(All.FindAllReferences(StatementMethods.GetAssignee()));
result.Add(StatementMethods);