CxList methods = Find_Methods();

result = methods.FindByMemberAccess("Database.ExecuteReader", false);
result.Add(methods.FindByMemberAccess("Database.ExecuteDataSet", false));
result.Add(methods.FindByMemberAccess("Database.ExecuteScalar", false));
result.Add(methods.FindByMemberAccess("OracleDatabase.ExecuteReader", false));
result.Add(methods.FindByMemberAccess("OracleDatabase.ExecuteDataSet", false));
result.Add(methods.FindByMemberAccess("OracleDatabase.ExecuteScalar", false));
result.Add(methods.FindByMemberAccess("SqlDatabase.ExecuteReader", false));
result.Add(methods.FindByMemberAccess("SqlDatabase.ExecuteDataSet", false));
result.Add(methods.FindByMemberAccess("SqlDatabase.ExecuteScalar", false));
result.Add(methods.FindByMemberAccess("GenericDatabase.ExecuteReader", false));
result.Add(methods.FindByMemberAccess("GenericDatabase.ExecuteDataSet", false));
result.Add(methods.FindByMemberAccess("GenericDatabase.ExecuteScalar", false));

//SqliteCommand
result.Add(methods.FindByMemberAccess("SqliteCommand.ExecuteScalar", false));
result.Add(methods.FindByMemberAccess("SqliteCommand.GetTableInfo", false));
result.Add(methods.FindByMemberAccess("SqliteCommand.Query", false));
result.Add(methods.FindByMemberAccess("SqliteCommand.DeferredQuery", false));