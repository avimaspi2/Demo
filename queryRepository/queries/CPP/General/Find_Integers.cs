/// <summary>
/// Find numbers (integer, double, etc.)
/// ---------------------------------------------------------------------
/// Based on the following references:
/// http://msdn.microsoft.com/en-us/library/aa273010(VS.60).aspx
/// http://www.chm.tu-dresden.de/edv/manuals/aix/libs/basetrf2/strtol.htm
/// http://www.mkssoftware.com/docs/man3/strtoq.3.asp
/// </summary>
///
CxList methods = Find_Methods();

// find all the integer references variables declared
CxList ints = Find_Builtin_Types() - Find_Builtin_Char_Types();

var intAliases = new string[]{
	"int8_t", "int16_t", "int32_t", "int64_t",
	"int_fast8_t", "int_fast16_t", "int_fast32_t", "int_fast64_t",
	"int_least8_t", "int_least16_t", "int_least32_t", "int_least64_t",
	"intmax_t", "uintmax_t",
	"uint8_t", "uint16_t", "uint32_t", "uint64_t",
	"uint_fast8_t", "uint_fast16_t", "uint_fast32_t", "uint_fast64_t",
	"uint_least8_t", "uint_least16_t", "uint_least32_t", "uint_least64_t"
	};
ints.Add(All.FindByTypes(intAliases));

// get all the methods that convert string to numbers (int, double, float and long)
List<string> convertMethods = new List<string>{
	// C Standard General Utilities Library (<cstdlib>)
	"strtod", "strtof", "strtol", "strtold", "strtoll",
	"strtoul", "strtoull", "strtoq", "strtouq", "rand",
	"atof", "atoi", "atol", "atoll",
		
	// Strings (<string>)
	"stoi", "stol", "stoul", "stoll","stoull",
	"stof", "stod", "stold", "sizeof", "length",
		
	// Wide characters (<cwchar>)
	"wcstod", "wcstof", "wcstol", "wcstold",
	"wcstoll", "wcstoul", "wcstoull"
};
CxList converted = methods.FindByShortNames(convertMethods) + Find_All_Strlen();

// get all the boolean conditions expressions
CxList binary = All.FindByType(typeof(BinaryExpr));
List<string> booleanOperators = new List<string>{
	"<", ">", "==", "!=", "<>", "<=", ">=", "||", "&&", "<=>"
};
CxList booleanConditions = binary.FindByShortNames(booleanOperators) + All.FindByType(typeof(UnaryExpr)).FindByShortName("Not");
CxList nonSanitizer = All.FindByType(typeof(AssignExpr)).GetByAncs(booleanConditions);
booleanConditions -= nonSanitizer.GetAncOfType(typeof(BinaryExpr));

// get all the methods that return integers
CxList methodInts = All.FindByReturnType("int") +
	All.FindByReturnType("long") +
	All.FindByReturnType("short") +
	All.FindByReturnType("float") + 
	All.FindByReturnType("double") +  
	All.FindByReturnType("bool")
;
methodInts = methods.FindAllReferences(methodInts);


result = ints + converted + booleanConditions + methodInts + Find_ESQL_Integers();