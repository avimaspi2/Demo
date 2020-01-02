List < string > inputMethods = new List<string>{
		"execSQL",
		"insert",
		"insertOrThrow",
		"insertWithOnConflict",
		"rawQuery",
		"rawQueryWithFactory",
		"replace",
		"replaceOrThrow",
		"update",
		"updateWithOnConflict"
		};

result = Find_SQLiteDatabase_Members().FindByShortNames(inputMethods);