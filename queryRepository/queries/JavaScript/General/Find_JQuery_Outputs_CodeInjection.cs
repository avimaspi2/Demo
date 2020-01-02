//Methods that can suffer Code injections

CxList Methods = Find_JQuery_Methods();

result = Methods.FindByShortNames(new List<string>{
		"load","globalEval","getScript"});