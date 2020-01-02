CxList ef = Find_DB_EF();

//// Sql queries done through Entity Framework are not sanitizer.
CxList excludeSanitizerMethods = ef.FindByShortNames(new List<string> {
		"SqlQuery",
		"FromSql",
		"ExecuteStoreCommand",
		"ExecuteStoreCommandAsync",
		"ExecuteStoreQuery",
		"ExecuteStoreQueryAsync",
		"ExecuteSqlCommand",
		"ExecuteSqlCommandAsync"
		});

ef.Add(All.GetParameters(excludeSanitizerMethods));

CxList allExcludeSanitizerParam = All.NewCxList();

foreach(CxList method in excludeSanitizerMethods){
	CxList listParam = All.GetParameters(method).FindByType("string");
	listParam.Add(All.GetParameters(method).FindByType(typeof(StringLiteral)));
	listParam.Add(All.GetParameters(method).FindByType(typeof(BinaryExpr)));
	int cmdSqlPosition = int.MaxValue;
	foreach(CxList paramVal in listParam){	
		int currentIndex = paramVal.GetAncOfType(typeof(Param)).GetIndexOfParameter();		
		cmdSqlPosition = currentIndex >= 0 && currentIndex < cmdSqlPosition ? currentIndex : cmdSqlPosition;	
	}

	cmdSqlPosition = cmdSqlPosition == int.MaxValue ? 0 : cmdSqlPosition;
	allExcludeSanitizerParam.Add(All.GetParameters(method, cmdSqlPosition));
	
}
excludeSanitizerMethods.Add(allExcludeSanitizerParam);

ef -= excludeSanitizerMethods;
result = ef;