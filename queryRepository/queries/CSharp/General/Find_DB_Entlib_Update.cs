/* UpdateDataSet() for GenericDataBase, SqlDataBase, DataBase and OracleDataBase
 */
CxList methods = Find_Methods();

result = methods.FindByMemberAccess("GenericDataBase.UpdateDataset");
result.Add(methods.FindByMemberAccess("SqlDataBase.UpdateDataset"));
result.Add(methods.FindByMemberAccess("OracleDataBase.UpdateDataset"));
result.Add(methods.FindByMemberAccess("DataBase.UpdateDataset"));