CxList allMethods = Find_Methods() + All.FindByType(typeof(MemberAccess));

CxList numberSanitizer = 
	allMethods.FindByShortName("*length*", false) + 
	allMethods.FindByShortName("*position*", false) +
	allMethods.FindByShortName("*size*", false) + 
	allMethods.FindByShortName("_count", false) + 
	allMethods.FindByShortName("average", false) + 
	allMethods.FindByShortName("cmp") +
	allMethods.FindByShortName("cos") +
	allMethods.FindByShortName("count", false) + 
	allMethods.FindByShortName("doubleval", false) +
	allMethods.FindByShortName("eq") +
	allMethods.FindByShortName("ge") +
	allMethods.FindByShortName("gmtime") +
	allMethods.FindByShortName("gt") +
	allMethods.FindByShortName("hex") +
	allMethods.FindByShortName("index", false) + 
	allMethods.FindByShortName("int", false) + 
	allMethods.FindByShortName("isalnum") +
	allMethods.FindByShortName("isalpha") +
	allMethods.FindByShortName("isdigit") +
	allMethods.FindByShortName("isgraph") +
	allMethods.FindByShortName("islower") +
	allMethods.FindByShortName("isprint") +
	allMethods.FindByShortName("ispunct") +
	allMethods.FindByShortName("isspace") +
	allMethods.FindByShortName("isupper") +
	allMethods.FindByShortName("isxdigit") +
	allMethods.FindByShortName("le") +
	allMethods.FindByShortName("localtime") +
	allMethods.FindByShortName("log") +
	allMethods.FindByShortName("lt") +
	allMethods.FindByShortName("maximum", false) + 
	allMethods.FindByShortName("minimum", false) + 
	allMethods.FindByShortName("ne") + 
	allMethods.FindByShortName("oct") + 
	allMethods.FindByShortName("ord") +
	allMethods.FindByShortName("round", false) + 
	allMethods.FindByShortName("sin") +
	allMethods.FindByShortName("strcoll") +
	allMethods.FindByShortName("strftime") +
	allMethods.FindByShortName("strxfrm") +
	allMethods.FindByShortName("sum", false) + 
	allMethods.FindByShortName("time") +
	allMethods.FindByShortName("times");

CxList binary = All.FindByType(typeof(BinaryExpr));
CxList comparisons = 
	binary.FindByShortName("==") +
	binary.FindByShortName("!=") +
	binary.FindByShortName("<") +
	binary.FindByShortName(">") +
	binary.FindByShortName("<=") +
	binary.FindByShortName(">=");
	
result = numberSanitizer + comparisons;