CxList registry = NodeJS_Find_Windows_Registry();

result = registry.FindByShortNames(new List<string>{
		"create", "remove", "erase", "set", "add"
		});