CxList registry = NodeJS_Find_Windows_Registry();

result = registry.FindByShortNames(new List<string>{
		"keys", "values", "get"
		});
result.Add(registry.FindByType(typeof(MemberAccess)));