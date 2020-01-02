// ExecuteNonQuery() and DoExecuteNonQuery() for GenericDatabase, SqlDatabase,
// DataBase and OracleDataBase
CxList methods = Find_Methods();

result = methods.FindByMemberAccess("GenericDataBase.ExecuteNonQuery");
result.Add(methods.FindByMemberAccess("GenericDataBase.DoExecuteNonQuery"));
result.Add(methods.FindByMemberAccess("SqlDataBase.ExecuteNonQuery"));
result.Add(methods.FindByMemberAccess("SqlDataBase.DoExecuteNonQuery"));
result.Add(methods.FindByMemberAccess("OracleDataBase.ExecuteNonQuery"));
result.Add(methods.FindByMemberAccess("OracleDataBase.DoExecuteNonQuery"));
result.Add(methods.FindByMemberAccess("DataBase.DoExecuteNonQuery"));
result.Add(methods.FindByMemberAccess("DataBase.ExecuteNonQuery"));