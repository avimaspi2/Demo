CxList methods = Find_Methods();

result = methods.FindByMemberAccess("IDbCommand.ExecuteNonQuery", false);
result.Add(methods.FindByMemberAccess("OdbcCommand.ExecuteNonQuery", false));
result.Add(methods.FindByMemberAccess("OleDbCommand.ExecuteNonQuery", false));
result.Add(methods.FindByMemberAccess("OracleCommand.ExecuteNonQuery", false));
result.Add(methods.FindByMemberAccess("SqlCommand.ExecuteNonQuery", false));
result.Add(methods.FindByMemberAccess("SqlCeCommand.ExecuteNonQuery", false));
result.Add(methods.FindByMemberAccess("OracleCommand.ExecuteOracleNonQuery", false));
result.Add(methods.FindByMemberAccess("DB22Command.ExecuteNonQuery", false));
result.Add(methods.FindByMemberAccess("UpdateCommand.ExecuteNonQuery", false));
result.Add(methods.FindByMemberAccess("DeleteCommand.ExecuteNonQuery", false));
result.Add(methods.FindByMemberAccess("InsertCommand.ExecuteNonQuery", false));
result.Add(methods.FindByMemberAccess("SelectCommand.ExecuteNonQuery", false));

result.Add(methods.FindByMemberAccess("SqliteCommand.ExecuteNonQuery", false));