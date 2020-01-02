CxList methods = Find_DB_In_Postgres();
methods.Add(Find_DB_Out_Postgres());

CxList stringLiteral = Find_String_Literal();
CxList binaryExprs = Find_BinaryExpr();

CxList allParams = Find_Param();

CxList connMethods = Find_DB_Conn_Postgres();
result.Add(connMethods.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly));

CxList execute = methods.FindByName("*.execute*");

//add to sanitizers string constants
CxList stringsLiteral = Find_Strings();

CxList allExecute = execute.FindByParameters(stringsLiteral.GetAncOfType(typeof(Param)));
CxList vunurableExecute = execute.FindByParameters(stringsLiteral.FindByFathers(Find_BinaryExpr()));
CxList executeSanitize = allExecute - vunurableExecute;
result.Add(executeSanitize);

//param as strings
result.Add(execute.FindByParameters(All.FindAllReferences(stringsLiteral.GetAssignee()).GetAncOfType(typeof(Param))));

//only the second parameter of execute_query is sanitized
result.Add(allParams.GetParameters(execute, 1));

//psycopg
result.Add(methods.FindByName("*.mogrify"));

//pygresql
CxList query = methods.FindByName("*.query");
//only the second parameter of query is sanitized
result.Add(allParams.GetParameters(query, 1));

result.Add(methods.FindByName("*.insert"));
result.Add(methods.FindByName("*.update"));

//pg-foundry
CxList prepare = methods.FindByName("*.prepare");
CxList variables = Find_UnknownReference();
variables.Add(Find_Declarators());

CxList prepVariable = variables.DataInfluencedBy(prepare);
CxList references = All.FindAllReferences(prepVariable);
CxList prepMethodsInvoke = references.FindByType(typeof(MethodInvokeExpr));
//All the parameters of methods that are using a prepare statement are sanitized
result.Add(allParams.GetParameters(prepMethodsInvoke));