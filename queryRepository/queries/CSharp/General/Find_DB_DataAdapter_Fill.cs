/* Fill() DB methods for DataAdapter, IDataAdapter,
 * IDbDataAdapter, SqlDataAdapter, SqlCeDataAdapter
 * OdbcDataAdapter, OleDbDataAdapter and OracleDataAdapter
 */
CxList methods = Find_Methods();

result = methods.FindByMemberAccess("DataAdapter.Fill");
result.Add(methods.FindByMemberAccess("IDataAdapter.Fill"));
result.Add(methods.FindByMemberAccess("IDbDataAdapter.Fill"));
result.Add(methods.FindByMemberAccess("SqlDataAdapter.Fill"));
result.Add(methods.FindByMemberAccess("SqlCeDataAdapter.Fill"));
result.Add(methods.FindByMemberAccess("OdbcDataAdapter.Fill"));
result.Add(methods.FindByMemberAccess("OleDbDataAdapter.Fill"));
result.Add(methods.FindByMemberAccess("OracleDataAdapter.Fill"));
/* Commented because are already caught by FindByMemberAccesses in SqlDataAdapter, due to the '*' by default in our solution.
 * If the '*' is removed from the solution, the next method are necessary in the query */ 
//result.Add(methods.FindByMemberAccess("NpgsqlDataAdapter.Fill"));