CxList unkr = Find_UnknownReference();
CxList associativeArrayExprs = Find_AssociativeArrayExpr();

CxList serverObject = Hapi_Find_Server_Instance();

CxList routing = serverObject.GetMembersOfTarget().FindByShortName("route");

CxList routesParams = unkr.GetParameters(routing);

// routes.push({...})
CxList routesAddedWithPush = associativeArrayExprs.GetParameters(unkr.FindAllReferences(routesParams).GetMembersOfTarget().FindByShortName("push"));

CxList associativeArrayExpr = associativeArrayExprs.GetByAncs(routing);

associativeArrayExpr.Add(associativeArrayExprs.GetByAncs(All.FindAllReferences(routesParams).GetAssigner()));
associativeArrayExpr -= associativeArrayExpr.GetByAncs(associativeArrayExpr.GetAncOfType(typeof(FieldDecl)));

associativeArrayExpr.Add(routesAddedWithPush);

result.Add(associativeArrayExpr);