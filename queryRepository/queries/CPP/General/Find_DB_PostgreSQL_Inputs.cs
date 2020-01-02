CxList sqlExecutionMethods = Find_DB_PostgreSQL_libpqxx();
sqlExecutionMethods.Add(Find_DB_PostgreSQL_libpq());

//based on https://www.postgresql.org/docs/9.1/reference.html
List<string> inputSqlCommands = new List<string> {
		"insert ", 
		"update ", 
		"load ", 
		"delete ", 
		"alter ", 
		"grant ", 
		"revoke ",  
		"truncate ",  
		"create ", 
		"copy ", 
		"drop ",
		"execute "
};

Func<string,List<string>, bool> containsSqlCommands = (string str, List<string> commands) => 
	commands
	.Select(c => str.IndexOf(c, System.StringComparison.CurrentCultureIgnoreCase))
	.Sum() != -(commands.Count);

CxList stringsWithInputCommands = All.FindByAbstractValue(
	absVal => 
	{
		return absVal is StringAbstractValue
			? containsSqlCommands((absVal as StringAbstractValue).Content, inputSqlCommands)
			: false;
	});

result = sqlExecutionMethods.DataInfluencedBy(stringsWithInputCommands)
	.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);