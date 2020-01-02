CxList methods = Find_Methods();

List<string> allMethod = new List<string> {
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
		"srandom",
		"CCRANDOM_MINUS1_1",
		"CCRANDOM_0_1",
		"RAND_bytes",
		"RAND_pseudo_bytes"
		};
result = methods.FindByShortNames(allMethod);