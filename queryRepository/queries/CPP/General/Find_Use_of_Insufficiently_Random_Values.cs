CxList methods = Find_Methods();

result.Add(methods.FindByShortNames(new List<string> {  
		"drand48", 
		"erand48", 
		"initstate", 
		"jrand48", 
		"lcong48", 
		"lrand48", 
		"mrand48", 
		"nrand48",
		"rand",
		"random",
		"seed48",
		"setstate",
		"srand",
		"srand",
		"srandom" }));