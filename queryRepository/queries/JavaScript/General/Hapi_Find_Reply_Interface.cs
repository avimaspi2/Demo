CxList methodInvoke = Find_Methods();
CxList routes = Hapi_Find_Routes();
CxList paramDecls = Find_ParamDecl();
CxList handlersInRoutes = Hapi_Find_Handler_Method_Under_Route(routes);

CxList allParams = paramDecls.GetParameters(handlersInRoutes.GetAssigner(Find_LambdaExpr()), 1);

result = All.FindAllReferences(allParams);
result.Add(methodInvoke.FindByShortName("reply").GetByAncs(handlersInRoutes));