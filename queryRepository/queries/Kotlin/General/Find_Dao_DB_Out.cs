List<string> daoOutputMethods = new List<string> {
		"countOf", 
		"queryForAll", 
		"executeRaw", 
		"executeRawNoArgs", 
		"queryRaw", 
		"queryRawValue",
		"extractId",
		"getTableName",
		"idExists",
		"isTableExists",
		"queryForEq",
		"queryForFieldValues",
		"queryForFieldValuesArgs",
		"queryForId",
		"queryForFirst",
		"queryForMatching",
		"queryForMatchingArgs",
		"queryForSameId",
		"queryRaw",
		"queryRawValue"
		};

result = Find_Dao_DB_Members().FindByShortNames(daoOutputMethods);