CxList sqlExecutionEethods = Find_DB_PostgreSQL_libpqxx();
sqlExecutionEethods.Add(Find_DB_PostgreSQL_libpq());

//based on https://www.postgresql.org/docs/9.1/reference.html
List<string> outputSqlCommands = new List<string> {
		"select ", 
		"show ",
		"vacuum ",
		"execute "
		};

Func<string, List<string>, bool> containsSqlCommands = 
	(string str, List<string> commands) => commands
		.Select(c => str.IndexOf(c, System.StringComparison.CurrentCultureIgnoreCase))
		.Sum() != -(commands.Count);

CxList stringsWithOutputCommands = All.FindByAbstractValue(
	absVal => 
	{
	return absVal is StringAbstractValue
		? containsSqlCommands((absVal as StringAbstractValue).Content, outputSqlCommands)
		: false;
	});

result = sqlExecutionEethods.DataInfluencedBy(stringsWithOutputCommands)
	.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);