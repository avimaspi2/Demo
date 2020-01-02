CxList allDbMethods = Find_DB_Methods().FindByType(typeof(MethodInvokeExpr));

CxList ChildrenOfTry = allDbMethods.GetByAncs(All.FindByType(typeof(TryCatchFinallyStmt)));


// locate error related methods to filter out
CxList errMethods = allDbMethods.FindByShortNames(new List<String>(){ "*error*", "*errno*", "sqlstate" });

// list of DB functions that throw exceptions
CxList dbMethods = allDbMethods - errMethods;
result = dbMethods;
result -= ChildrenOfTry;