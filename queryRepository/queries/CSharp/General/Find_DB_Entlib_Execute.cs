/* ExecuteReader(), ExecuteDataSet() and ExecuteScalar() for DataBase, OracleDataBase,
 * SqlDataBase and GenericDataBase
 */

CxList methods = Find_Methods();

result = methods.FindByMemberAccess("DataBase.ExecuteReader");
result.Add(methods.FindByMemberAccess("DataBase.ExecuteDataSet"));
result.Add(methods.FindByMemberAccess("DataBase.ExecuteScalar"));
result.Add(methods.FindByMemberAccess("OracleDataBase.ExecuteReader"));
result.Add(methods.FindByMemberAccess("OracleDataBase.ExecuteDataSet"));
result.Add(methods.FindByMemberAccess("OracleDataBase.ExecuteScalar"));
result.Add(methods.FindByMemberAccess("SqlDataBase.ExecuteReader"));
result.Add(methods.FindByMemberAccess("SqlDataBase.ExecuteDataSet"));
result.Add(methods.FindByMemberAccess("SqlDataBase.ExecuteScalar"));
result.Add(methods.FindByMemberAccess("GenericDataBase.ExecuteReader"));
result.Add(methods.FindByMemberAccess("GenericDataBase.ExecuteDataSet"));
result.Add(methods.FindByMemberAccess("GenericDataBase.ExecuteScalar"));

//SqliteCommand
result.Add(methods.FindByMemberAccess("SqliteCommand.ExecuteScalar"));
result.Add(methods.FindByMemberAccess("SqliteCommand.GetTableInfo "));
result.Add(methods.FindByMemberAccess("SqliteCommand.Query"));
result.Add(methods.FindByMemberAccess("SqliteCommand.DeferredQuery"));