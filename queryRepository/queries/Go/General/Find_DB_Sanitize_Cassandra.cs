//cassandra 
//sanitized parameters
CxList methods = Find_Methods();
CxList execMethods = methods.FindByShortName("Exec");
execMethods.Add(methods.FindByShortName("Consistency"));
CxList methodQuery = methods.FindByShortName("Query");

CxList listQuery = methodQuery.DataInfluencingOn(execMethods);
CxList myParam = Find_Param();
CxList myAllParameter = All.GetParameters(listQuery);
CxList myFirstParameter = All.GetParameters(listQuery, 0);
myFirstParameter.Add(myParam);
result.Add(myAllParameter - myFirstParameter);

//get methods sanitized 
CxList sqlStrings = Find_Sql_Strings();
CxList myPlus = Find_BinaryExpr();
CxList trash = All.NewCxList();
foreach(CxList plus in myPlus){
	BinaryExpr graph = plus.TryGetCSharpGraph<BinaryExpr>();
	if(graph.Operator == BinaryOperator.Add){
		trash.Add(plus);
	}
}

result.Add(methodQuery.FindByParameters(sqlStrings - sqlStrings.InfluencingOn(trash)));