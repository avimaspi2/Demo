CxList SqlAttr = All.FindByName("*SqlProcedure", false).FindByType(typeof(CustomAttribute));
CxList SqlProcedure = SqlAttr.GetFathers().GetFathers().FindByType(typeof(MethodDecl));

result = All.FindByFathers(All.FindByFathers(SqlProcedure)).FindByType(typeof(ParamDecl));