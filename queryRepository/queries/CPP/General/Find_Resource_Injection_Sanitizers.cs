CxList sanitizers = Find_Methods().FindByShortNames(new List<string>(){"regex_match","regex_search","strcmp","compare","find"});
result = All.GetByAncs(All.GetParameters(sanitizers));

result.Add(All.FindByFathers(Find_BinaryExpr().FindByShortName("==")));