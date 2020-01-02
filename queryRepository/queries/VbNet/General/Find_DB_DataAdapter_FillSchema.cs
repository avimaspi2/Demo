CxList methods = Find_Methods();

result = methods.FindByMemberAccess("DataAdapter.FillSchema", false);
result.Add(methods.FindByMemberAccess("IDataAdapter.FillSchema", false));
result.Add(methods.FindByMemberAccess("IDbDataAdapter.FillSchema", false));
result.Add(methods.FindByMemberAccess("SqlDataAdapter.FillSchema", false));
result.Add(methods.FindByMemberAccess("SqlCeDataAdapter.FillSchema", false));
result.Add(methods.FindByMemberAccess("OdbcDataAdapter.FillSchema", false));
result.Add(methods.FindByMemberAccess("OleDbDataAdapter.FillSchema", false));
result.Add(methods.FindByMemberAccess("OracleDataAdapter.FillSchema", false));