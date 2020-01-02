CxList methods = Find_Methods();

result = methods.FindByMemberAccess("DataBase.LoadDataSet", false);
result.Add(methods.FindByMemberAccess("GenericDataBase.LoadDataSet", false));
result.Add(methods.FindByMemberAccess("SqlDatabase.LoadDataSet", false));
result.Add(methods.FindByMemberAccess("OracleDatabase.LoadDataSet", false));