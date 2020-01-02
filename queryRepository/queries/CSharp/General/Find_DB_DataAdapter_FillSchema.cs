/* FillSchema() DB methods for DataAdapter, IDataAdapter,
 * IDbDataAdapter, SqlDataAdapter, SqlCeDataAdapter
 * OdbcDataAdapter, OleDbDataAdapter and OracleDataAdapter
 */

CxList methods = Find_Methods();

result = methods.FindByMemberAccess("DataAdapter.FillSchema");
result.Add(methods.FindByMemberAccess("IDataAdapter.FillSchema"));
result.Add(methods.FindByMemberAccess("IDbDataAdapter.FillSchema"));
result.Add(methods.FindByMemberAccess("SqlDataAdapter.FillSchema"));
result.Add(methods.FindByMemberAccess("SqlCeDataAdapter.FillSchema"));
result.Add(methods.FindByMemberAccess("OdbcDataAdapter.FillSchema"));
result.Add(methods.FindByMemberAccess("OleDbDataAdapter.FillSchema"));
result.Add(methods.FindByMemberAccess("OracleDataAdapter.FillSchema"));
/* Commented because are already caught by FindByMemberAccesses in SqlDataAdapter, due to the '*' by default in our solution.
 * If the '*' is removed from the solution, the next method are necessary in the query */ 
//result.Add(methods.FindByMemberAccess("NpgsqlDataAdapter.FillSchema"));