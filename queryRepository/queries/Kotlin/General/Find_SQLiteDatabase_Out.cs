List < string > outputMethods = new List<string>{
		"execSQL",
		"rawQuery",
		"rawQueryWithFactory",
		"findEditTable",
		"getAttachedDbs",
		"getMaximumSize",
		"getPageSize",
		"getPath",
		"getSyncedTables",
		"getVersion",
		"query",
		"queryWithFactory"
		};

result = Find_SQLiteDatabase_Members().FindByShortNames(outputMethods);