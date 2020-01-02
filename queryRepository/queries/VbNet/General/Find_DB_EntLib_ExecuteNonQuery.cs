CxList methods = Find_Methods();

result = methods.FindByMemberAccess("GenericDatabase.ExecuteNonQuery", false);
result.Add(methods.FindByMemberAccess("GenericDatabase.doExecuteNonQuery", false));
result.Add(methods.FindByMemberAccess("SqlDatabase.ExecuteNonQuery", false));
result.Add(methods.FindByMemberAccess("SqlDatabase.DoExecuteNonQuery", false));
result.Add(methods.FindByMemberAccess("OracleDatabase.ExecuteNonQuery", false));
result.Add(methods.FindByMemberAccess("OracleDatabase.doExecuteNonQuery", false));
result.Add(methods.FindByMemberAccess("Database.doExecuteNonQuery", false));
result.Add(methods.FindByMemberAccess("Database.ExecuteNonQuery", false));