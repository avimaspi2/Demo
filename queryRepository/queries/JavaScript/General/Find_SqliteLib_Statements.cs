CxList dbs = Find_SqliteLib_Databases();
var dbMembers = new List<string>{"prepare"};
CxList Statements = Get_Promise_Results(dbs.GetMembersOfTarget().FindByShortNames(dbMembers));
result.Add( All.FindAllReferences(Statements) );