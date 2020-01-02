CxList methods = Find_Methods();

result = methods.FindByMemberAccess("GenericDatabase.UpdateDataSet", false);
result.Add(methods.FindByMemberAccess("SqlDatabase.UpdateDataSet", false));
result.Add(methods.FindByMemberAccess("OracleDatabase.UpdateDataSet", false));
result.Add(methods.FindByMemberAccess("DataBase.UpdateDataSet", false));