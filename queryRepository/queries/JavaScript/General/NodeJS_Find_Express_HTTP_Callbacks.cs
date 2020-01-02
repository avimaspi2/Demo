CxList nodeJsCallbacks = NodeJS_Find_ExpressMembers();

List<string> httpMemberNames = new List<string>{"get", "METHOD", "param", "post", "put", "route", "use", "param", "delete"};

CxList expressJsHttpMembers = nodeJsCallbacks.FindByShortNames(httpMemberNames);
CxList urls = All.GetParameters(expressJsHttpMembers, 0);
CxList everyParam = All.GetParameters(expressJsHttpMembers);
CxList callbacks = everyParam - urls;

CxList callbackDefs = All.FindDefinition(callbacks);
CxList returnStmts = Find_ReturnStmt().FindByFathers(Find_StatementCollection().FindByFathers(callbackDefs));
CxList returnedCallbacks = Find_LambdaExpr().FindByFathers(returnStmts);

result = callbackDefs;
result.Add(returnedCallbacks);