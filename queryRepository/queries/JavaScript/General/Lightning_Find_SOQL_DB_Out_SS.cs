CxList methods = Get_Apex().FindByType(typeof(MethodInvokeExpr));
CxList res = methods.FindByShortName("select");
res.Add(methods.FindByMemberAccess("database.query"));
res.Add(methods.FindByMemberAccess("search.query"));
result = res;