CxList inputs = Find_Interactive_Inputs();
CxList methods = Find_Methods();

CxList include = methods.FindByShortNames(new List<String>(){ "unserialize",
		// Exploit available in PHP5.3.7/5.3.8
		"is_a" }, false);

CxList generalSanitizer = methods.FindByShortName("serialize", false);
CxList sanitized = Find_Integers() + generalSanitizer;
result = include.InfluencedByAndNotSanitized(inputs, sanitized);