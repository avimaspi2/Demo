CxList methods = Find_Methods();

result = methods.FindByMemberAccess("DataAdapter.Fill", false);
result.Add(methods.FindByMemberAccess("IDataAdapter.Fill", false));
result.Add(methods.FindByMemberAccess("IDbDataAdapter.Fill", false));
result.Add(methods.FindByMemberAccess("SqlDataAdapter.Fill", false));
result.Add(methods.FindByMemberAccess("SqlCeDataAdapter.Fill", false));
result.Add(methods.FindByMemberAccess("OdbcDataAdapter.Fill", false));
result.Add(methods.FindByMemberAccess("OleDbDataAdapter.Fill", false));
result.Add(methods.FindByMemberAccess("OracleDataAdapter.Fill", false));