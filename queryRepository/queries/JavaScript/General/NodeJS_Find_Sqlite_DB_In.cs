CxList methods = NodeJS_Find_Sqlite_commands().FindByShortNames(new List<string>{"run","get","all","each","exec","prepare"});
//Sanitizers are also considered inputs to a DB
methods.Add(NodeJS_Find_Sqlite_sanitizers());

CxList parameters = Find_Param();
CxList lambdas = Find_LambdaExpr();

result = All.GetParameters(methods) - parameters - lambdas;