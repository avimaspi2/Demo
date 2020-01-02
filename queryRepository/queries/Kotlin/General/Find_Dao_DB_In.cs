List<string> daoMethods = new List<string> {
		"update", 
		"createOrUpdate", 
		"queryForAll", 
		"create", 
		"createIfNotExists", 
		"executeRaw", 
		"executeRawNoArgs", 
		"queryRaw", 
		"queryRawValue",
		"updateRaw",
		"delete",
		"deleteById",
		"deleteIds",
		"rawQuery"
		};

result = Find_Dao_DB_Members().FindByShortNames(daoMethods);