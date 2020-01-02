/* This query will return all of the request objects. */
CxList serverObject = Hapi_Find_Server_Instance();
CxList routes = Hapi_Find_Routes();
CxList allHandlersUnderRoutes = Hapi_Find_Handler_Method_Under_Route(routes);
CxList lambdaExprs = Find_LambdaExpr();
CxList paramDecls = Find_ParamDecl();
CxList unkRef = Find_UnknownReference();
CxList methodDecls = Find_MethodDecls();

CxList requestsOnHandlerFunctions =
	unkRef.FindAllReferences(paramDecls.GetParameters(allHandlersUnderRoutes.GetAssigner(lambdaExprs), 0));
result.Add(requestsOnHandlerFunctions);

//Get hadlers that are unknown references in handler key
CxList unknownRefHandlers = allHandlersUnderRoutes.GetAssigner(unkRef);
CxList methodDeclsHandlers = methodDecls.FindDefinition(unknownRefHandlers);

//Add references to methodDecls
result.Add(unkRef.FindAllReferences(paramDecls.GetParameters(methodDeclsHandlers, 0)));
CxList assignmetLambdas = unkRef.FindAllReferences(unknownRefHandlers).GetAssigner(lambdaExprs);
//Add references to unknown reference's lambdas
result.Add(unkRef.FindAllReferences(paramDecls.GetParameters(assignmetLambdas, 0)));

//Add functions which are under .ext: server.ext('onPreResponse', function(request, reply) {..})
CxList extFuncs = serverObject.GetMembersOfTarget().FindByShortName("ext");
CxList extFuncsLambdaParameter = lambdaExprs.GetParameters(extFuncs);
result.Add(All.FindAllReferences(paramDecls.GetParameters(extFuncsLambdaParameter, 0)));

//Add request event
CxList serverOnEvent = serverObject.GetRightmostMember().FindByShortName("on");
CxList serverOnEventLambdaParam = lambdaExprs.GetParameters(serverOnEvent);
result.Add(All.FindAllReferences(paramDecls.GetParameters(serverOnEventLambdaParam, 0)));