// Find numbers (integer, double, etc.)

List<string> relevantTypes = new List<string>() {"int", "integer", "float", "bool", "boolean", "double", "long"};

CxList cast = Find_CastExpr();
CxList numberCast = All.NewCxList();
foreach(CxList l in cast)
{
	CastExpr ce = l.TryGetCSharpGraph<CastExpr>();
	String castType = ce.TargetType.TypeName.ToLower();
	if(relevantTypes.Contains(castType))
	{
		numberCast.Add(All.FindByFathers(l));
	}
}

List <string> numberSanitizerList = new List<string>()
	{"round", "average", "maximum", "minimum", "sum", "doubleval",
		"strlen", "intval", "count*", "*size*", "*length*", "*position*"};
	
CxList numberSanitizer = All.FindByShortNames(numberSanitizerList, false);

result.Add(numberSanitizer);
result.Add(numberCast);
// Based on the following references:
// http://msdn.microsoft.com/en-us/library/aa273010(VS.60).aspx
// http://www.chm.tu-dresden.de/edv/manuals/aix/libs/basetrf2/strtol.htm
// http://www.mkssoftware.com/docs/man3/strtoq.3.asp

CxList builtInTypes = Find_Builtin_Types();
CxList ints = All.NewCxList();
ints.Add(builtInTypes);

CxList builtInTypesToremove = builtInTypes.FindByType("char");
builtInTypesToremove.Add(builtInTypes.FindByReturnType("char"));

ints -= builtInTypesToremove;


List <string> convertedList = new List<string>()
	{"atoi", "atof", "atol", "strtod", "wcstod", "strtol", "wcstol", "strtoul",
		"wcstoul", "strtoll", "strtoull", "strtoq", "strtouq", "sizeof", "length"};

CxList methods = Find_Methods();
CxList converted = methods.FindByShortNames(convertedList);
converted.Add(Find_All_Strlen());

CxList binary = Find_BinaryExpr();

List<string> booleanConditionsList = new List<string>()
	{"<", ">", "==", "!=", "<>", "<=", ">=", "||", "&&"};

CxList booleanConditions = binary.FindByShortNames(booleanConditionsList); 
booleanConditions.Add(Find_Unarys().FindByShortName("Not"));

List<string> conditionsList = new List<string>()
	{"@\"==\"", "\"==\"", "@\"!=\"", "\"!=\""};

booleanConditions.Add(All.FindByShortNames(conditionsList));

CxList nonSanitizer = Find_AssignExpr().GetByAncs(booleanConditions);
booleanConditions -= nonSanitizer.GetAncOfType(typeof(BinaryExpr));

CxList methodInts = All.FindByReturnType("int");
methodInts.Add(All.FindByReturnType("long"));
methodInts.Add(All.FindByReturnType("short")); 
methodInts.Add(All.FindByReturnType("float")); 
methodInts.Add(All.FindByReturnType("double"));  
methodInts.Add(All.FindByReturnType("bool"));

methodInts = methods.FindAllReferences(methodInts);

string[] numberTypes = new string[] { 		
		"NSNumber", "NSInteger","CGFloat", "NSUInteger",
		"NSDecimal", "NSDecimalNumber","NSTimeInterval","TimeInterval"
		};

CxList objCNumbers = All.FindByTypes(numberTypes);

result.Add(ints);
result.Add(converted);
result.Add(booleanConditions);
result.Add(methodInts);
result.Add(objCNumbers);

// Debugging Identifiers
List<string> debugIdentifiers = new List<string> { 		
		"__LINE__", "__COLUMN__",
		"#line", "#column"
		};

result.Add(Find_UnknownReference().FindByShortNames(debugIdentifiers));