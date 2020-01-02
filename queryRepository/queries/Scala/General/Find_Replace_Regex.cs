CxList methods = Find_Methods();

result.Add(methods.FindByShortNames(new List<string>{"replaceAll", "replaceFirst",
		"replaceAllIn", "replaceFirstIn", "replaceSomeIn"}));

result.Add(methods.FindByMemberAccess("StringUtils.replace*"));
result.Add(methods.FindByMemberAccess("StringUtils.overlay"));