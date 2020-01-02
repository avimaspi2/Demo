CxList methods = Find_Methods();

result = methods.FindByMemberAccess("DataAdapter.Update", false);
result.Add(methods.FindByMemberAccess("IDataAdapter.Update", false));
result.Add(methods.FindByMemberAccess("IDbDataAdapter.Update", false));
result.Add(methods.FindByMemberAccess("SqlDataAdapter.Update", false));
result.Add(methods.FindByMemberAccess("SqlCeDataAdapter.Update", false));
result.Add(methods.FindByMemberAccess("OdbcDataAdapter.Update", false));
result.Add(methods.FindByMemberAccess("OleDbDataAdapter.Update", false));
result.Add(methods.FindByMemberAccess("OracleDataAdapter.Update", false));