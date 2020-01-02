CxList logs = general.Find_Log_Outputs();
CxList catchs = Find_Catch();
CxList catchScope = All.GetByAncs(catchs);

//Get methods's parameters inside a catch that influences on the logs functions
CxList catchMethods = catchScope.FindByType(typeof(MethodInvokeExpr));
CxList catchMethodsParameters = All.GetParameters(catchMethods);
CxList parametersInfluence = logs.DataInfluencedBy(catchMethodsParameters);
CxList startNodes = parametersInfluence.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);

CxList logsInCatch = catchScope * logs;

CxList sanitizedScopes = All.NewCxList();
sanitizedScopes.Add(logsInCatch);
sanitizedScopes.Add(startNodes);
sanitizedScopes.Add(Find_ThrowStmt());

CxList catchsToRemove = sanitizedScopes.GetAncOfType(typeof(Catch));
CxList catchsWithNoLogs = catchs - catchsToRemove;
result = catchsWithNoLogs;