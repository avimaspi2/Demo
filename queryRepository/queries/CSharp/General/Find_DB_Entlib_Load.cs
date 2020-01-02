//LoadDataSet() for DataBase, OracleDatabase, GenericDataBase and SqlDatabase

CxList methods = Find_Methods();

result = methods.FindByMemberAccess("DataBase.LoadDataSet");
result.Add(methods.FindByMemberAccess("GenericDataBase.LoadDataSet"));
result.Add(methods.FindByMemberAccess("SqlDataBase.LoadDataSet"));
result.Add(methods.FindByMemberAccess("OracleDataBase.LoadDataSet"));