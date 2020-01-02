/*
 * This query looks for PostgreSQL DB methods that allow
 * any kind of query execution (SELECT, INSERT, UPDATE, DELETE ...)
*/

CxList methods = Find_Methods();

CxList membersOfPGStatements = methods.FindByMemberAccess("PgStatement.*");
CxList membersOfPGConnections = methods.FindByMemberAccess("PgConnection.*");

result = membersOfPGStatements.FindByShortNames(new List<string>(){"execute", "executeQuery", "executeBatch", "executeLargeBatch", "executeWithFlags"});
result.Add( membersOfPGConnections.FindByShortNames(new List<string>(){"execSQLQuery"}) );
result.Add(methods.FindByMemberAccess("QueryExecutorImpl.execute"));