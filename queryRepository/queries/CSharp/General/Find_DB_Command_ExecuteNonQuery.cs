// ExecuteNonQuery() methods for IDbCommand, OdbcCommand, OleDbCommand,
// OracleCommand, SqlCommand, SqlCeCommand, Db2Command, UdapteCommand, 
// DeleteCommand, InsertCommand, SelectCommand, EntityCommand  and SqliteCommand
// ExecuteOracleNonQuery() for OracleCommand
CxList methods = Find_Methods();

//Sync methods
result = methods.FindByMemberAccess("IDbCommand.ExecuteNonQuery");
result.Add(methods.FindByMemberAccess("Db2Command.ExecuteNonQuery"));
result.Add(methods.FindByMemberAccess("DeleteCommand.ExecuteNonQuery"));
result.Add(methods.FindByMemberAccess("EntityCommand.ExecuteNonQuery"));
result.Add(methods.FindByMemberAccess("InsertCommand.ExecuteNonQuery"));
result.Add(methods.FindByMemberAccess("OdbcCommand.ExecuteNonQuery"));
result.Add(methods.FindByMemberAccess("OleDbCommand.ExecuteNonQuery"));
result.Add(methods.FindByMemberAccess("OracleCommand.ExecuteNonQuery"));
result.Add(methods.FindByMemberAccess("OracleCommand.ExecuteOracleNonQuery"));
result.Add(methods.FindByMemberAccess("SqlCeCommand.ExecuteNonQuery"));
result.Add(methods.FindByMemberAccess("SqlCommand.ExecuteNonQuery"));
result.Add(methods.FindByMemberAccess("SQLiteCommand.ExecuteNonQuery"));
result.Add(methods.FindByMemberAccess("UpdateCommand.ExecuteNonQuery"));
/* Commented because are already caught by FindByMemberAccesses in SqlCommand, due to the '*' by default in our solution.
 * If the '*' is removed from the solution, the next method are necessary in the query */ 
//result.Add(methods.FindByMemberAccess("NpgsqlCommand.ExecuteNonQuery"));

//Async methods
result.Add(methods.FindByMemberAccess("EntityCommand.ExecuteNonQueryAsync"));
result.Add(methods.FindByMemberAccess("OdbcCommand.ExecuteNonQueryAsync"));
result.Add(methods.FindByMemberAccess("OleDbCommand.ExecuteNonQueryAsync"));
result.Add(methods.FindByMemberAccess("OracleCommand.ExecuteNonQueryAsync"));
result.Add(methods.FindByMemberAccess("SQLiteCommand.ExecuteNonQueryAsync"));
result.Add(methods.FindByMemberAccess("SqlCommand.ExecuteNonQueryAsync"));
/* Commented because are already caught by FindByMemberAccesses in SqlCommand, due to the '*' by default in our solution.
 * If the '*' is removed from the solution, the next method are necessary in the query */ 
//result.Add(methods.FindByMemberAccess("NpgsqlCommand.ExecuteNonQueryAsync"));

result.Add(Find_DB_ImplicitTypedCommands().GetMembersOfTarget()
	.FindByShortNames(new List<string>{"ExecuteNonQuery", "ExecuteNonQueryAsync"}));