/* Update() DB methods for DataAdapter, IDataAdapter,
 * IDbDataAdapter, SqlDataAdapter, SqlCeDataAdapter
 * OdbcDataAdapter, OleDbDataAdapter and OracleDataAdapter
 */

CxList methods = Find_Methods();

result = methods.FindByMemberAccess("DataAdapter.Update");
result.Add(methods.FindByMemberAccess("IDataAdapter.Update"));
result.Add(methods.FindByMemberAccess("IDbDataAdapter.Update"));
result.Add(methods.FindByMemberAccess("SqlDataAdapter.Update"));
result.Add(methods.FindByMemberAccess("SqlCeDataAdapter.Update"));
result.Add(methods.FindByMemberAccess("OdbcDataAdapter.Update"));
result.Add(methods.FindByMemberAccess("OleDbDataAdapter.Update"));
result.Add(methods.FindByMemberAccess("OracleDataAdapter.Update"));
/* Commented because are already caught by FindByMemberAccesses in SqlDataAdapter, due to the '*' by default in our solution.
 * If the '*' is removed from the solution, the next method is necessary in the query */ 
//result.Add(methods.FindByMemberAccess("NpgsqlDataAdapter.Update"));