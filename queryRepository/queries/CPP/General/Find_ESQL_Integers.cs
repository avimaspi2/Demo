// Searches for Integer nodes comming from the ESQL/C code
CxList methods = Find_Methods();
CxList unkownReferences = Find_Unknown_References();

// methods which return an integer
List<string> integer_methods = new List<string>{
		"bycmpr", "byleng", "deccmp", "ifx_getserial8",
		"rdayofweek", "stcmpr", "stleng", "rtypname"
		};
result.Add(methods.FindByShortNames(integer_methods, false));

// methods which return by reference an integer on second parameter
List<string> secondParamMethods = new List<string>{
		"bigintcvdbl", "bigintcvdec", "bigintcvflt", "bigintcvifx_int8", "bigintcvint2",
		"bigintcvint4", "biginttodbl", "biginttodec", "biginttoflt", "biginttoifx_int8",
		"biginttoint2", "biginttoint4", "deccopy", "deccvdbl", "deccvflt", "deccvint",
		"deccvlong", "decround", "dectodbl", "dectoint", "dectolong", "dectrunc", 
		"ifx_int8cmp", "ifx_int8copy", "ifx_int8cvdbl", "ifx_int8cvdec", "ifx_int8cvflt",
		"ifx_int8cvint", "ifx_int8cvlong", "ifx_int8todbl", "ifx_int8todec", "ifx_int8toflt",
		"ifx_int8toint", "ifx_int8tolong", "rstod", "rstoi", "rstol", "stcopy"
		};
CxList secondParam = All.GetParameters(methods.FindByShortNames(secondParamMethods, false), 1);
result.Add(unkownReferences.FindByFathers(secondParam));

// methods which return by reference an integer on third parameter
List<string> thirdParamMethods = new List<string>{
		"bigintcvasc", "decadd", "deccvasc", "decdiv", "decmul", 
		"decsub", "ifx_int8add", "ifx_int8cvasc", "ifx_int8div",
		"ifx_int8mul", "ifx_int8sub", "rfmtlong"
		};
CxList thirdParam = All.GetParameters(methods.FindByShortNames(thirdParamMethods, false), 2);
result.Add(unkownReferences.FindByFathers(thirdParam));