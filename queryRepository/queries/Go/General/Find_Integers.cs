/*
	Search for numeric references, type conversions, constants, methods (project-based and from packages) and operators.
*/
CxList methods = Find_Methods();
CxList variables = Find_UnknownReferences();

string[] numericTypes = new string[] {
	"int", "int8", "int16", "int32", "int64",					// "int*"
	"uint", "uint8", "uint16", "uint32", "uint64", "uintptr",	// "uint*"
	"float32", "float64",										// "float*"
	"complex64", "complex128",									// "complex*"
	"rune", 													// alias for int32
	"byte",														// alias for uint8
	"bool",
	"ComplexType", "FloatType", "IntegerType"					// builtin
	}; 

// https://golang.org/pkg/math/big/
string[] mathBigPkgTypes = new string[] {"Float", "Int", "Rat", "Accuracy", "RoundingMode", "Word"}; 

// https://golang.org/pkg/time/
string[] timePkgTypes = new string[] {"Duration", "Location", "Month", "Ticker", "Time", "Timer", "Weekday"}; 

List<string> allTypes = new List<string>(numericTypes);
allTypes.AddRange(mathBigPkgTypes);
allTypes.AddRange(timePkgTypes);

string[] allTypesArray = allTypes.ToArray();

// Find numeric references declarators
CxList numericReferences = variables.FindByTypes(allTypesArray, true);

// Type conversions: T(v)
CxList typeConversions = methods.FindByShortNames(new List<string>(numericTypes));
// Check if type conversion methods have definition.
CxList typeConversionDefinitions = All.FindDefinition(typeConversions);
// Internal functions with same name as type conversions could not be sanitizers and are not type conversions.
typeConversions -= All.FindAllReferences(typeConversionDefinitions);

// Constants (eg: const Pi = 3.14) and Numeric constants (eg: const Big = 1 << 100)
CxList constantsDecl = Find_ConstantDeclStmt();
CxList numericConstants = constantsDecl.FindByTypes(allTypesArray);
CxList constants = numericConstants;

CxList mathBigPkgConstants = Find_Members_MathBig();
constants.Add(mathBigPkgConstants);

CxList timePkgConstants = Find_Members_Time();
constants.Add(timePkgConstants);

// Internal methods returning numeric values
CxList methodDefinitions = Find_MethodDecls();

CxList internalMethodsDef = All.NewCxList();
foreach(string type in allTypes) {
	internalMethodsDef.Add(methodDefinitions.FindByMethodReturnType(type));
}

// Intersection between methods and references of numeric method definitions.
CxList internalMethods = methods * All.FindAllReferences(internalMethodsDef);

// Methods from external packages 

// https://golang.org/pkg/builtin/
List<string> builtinMethodsList = new List<string>(){
		"cap", "complex", "copy", "imag", "len", "real"
		};

CxList builtinMethods = methods.FindByShortNames(builtinMethodsList);
// Check if builtin methods have definition.
CxList builtinDefinitions = All.FindDefinition(builtinMethods);
// Internal functions with same name as builtin could not be sanitizers and are not builtin functions.
builtinMethods -= All.FindAllReferences(builtinDefinitions);

CxList externalMethods = builtinMethods;

// https://golang.org/pkg/math/
CxList mathPkgMembers = Find_Members_Math();
externalMethods.Add(mathPkgMembers);

// https://golang.org/pkg/math/cmplx/
CxList mathCmplxPkgMembers = Find_Members_MathCmplx();
externalMethods.Add(mathCmplxPkgMembers);

// https://golang.org/pkg/math/rand/
CxList mathRandPkgMembers = Find_Members_MathRand();
externalMethods.Add(mathRandPkgMembers);

// https://golang.org/pkg/math/big/
List<string> mathBigPkgMethods = new List<string>{ "Jacobi", "New*", "ParseFloat" };
CxList mathBigPkgMembers = All.FindByMemberAccess("\"math/big\".*").FindByShortNames(mathBigPkgMethods);
externalMethods.Add(mathBigPkgMembers);

CxList mathBigPkgFloatType = numericReferences.FindByType("Float");
CxList mathBigPkgFloatMembers = mathBigPkgFloatType.GetMembersOfTarget();
List<string> mathBigPkgFloatMethods = new List<string>(){
	"Abs", "Acc", "Add", "Cmp", "Copy", "Float*", "Int*", "IsIn*", 
	"MantExp", "MinPrec", "Mode", "Mul", "Neg", "Parse", 
	"Prec", "Quo", "Rat", "Set*", "Sign*", "Sub", "Uint64"
	};
externalMethods.Add(mathBigPkgFloatMembers.FindByShortNames(mathBigPkgFloatMethods));

// This method catch also "int" identifiers (only "Int" needed)
CxList mathBigPkgIntType = numericReferences.FindByType("Int");
CxList mathBigPkgIntMembers = mathBigPkgIntType.GetMembersOfTarget();
List<string> mathBigPkgIntMethods = new List<string>(){
	"Abs", "Add", "And*", "Binomial", "Bit", "BitLen", "Cmp", "Div*", "Exp", 
	"GCD", "Int64", "Lsh", "Mod*", "Mul*", "Neg",  "Not", "Or", "ProbablyPrime", 
	"Quo*", "Rand", "Rem", "Rsh", "Set*", "Sign", "Sqrt", "Sub", "Uint64", "Xor"
	};
externalMethods.Add(mathBigPkgIntMembers.FindByShortNames(mathBigPkgIntMethods));

CxList mathBigPkgRatType = numericReferences.FindByType("Rat");
CxList mathBigPkgRatMembers = mathBigPkgRatType.GetMembersOfTarget();
List<string> mathBigPkgRatMethods = new List<string>(){
	"Abs", "Add", "Cmp", "Denom", "Float32", "Float64", "Inv", 
	"IsIn*", "Mul", "Neg", "Num", "Quo", "Set*", "Sign", "Sub",
	};
externalMethods.Add(mathBigPkgRatMembers.FindByShortNames(mathBigPkgRatMethods));

// https://golang.org/pkg/strconv/
CxList strconvPkgMembers = Find_Members_Strconv();
externalMethods.Add(strconvPkgMembers);

// https://golang.org/pkg/strings/
CxList stringsPkgMembers = Find_Members_Strings();
externalMethods.Add(stringsPkgMembers);

CxList stringPkgReaderMember = Find_Members_StringFile();
CxList stringsPkgReaderAssignees = stringPkgReaderMember.GetAssignee();
CxList stringsPkgReaderReferences = All.FindAllReferences(stringsPkgReaderAssignees);
CxList stringsPkgReaderMembers = stringsPkgReaderReferences.GetMembersOfTarget();

List<string> stringsPkgReaderMethods = new List<string>(){
		"Len", "Read*", "Seek", "Size", "WriteTo"
		};
externalMethods.Add(stringsPkgReaderMembers.FindByShortNames(stringsPkgReaderMethods));

// https://golang.org/pkg/database/sql/
CxList databaseSqlMethods = Find_Members_Database();
CxList databasePkgAssignees = databaseSqlMethods.GetFathers().GetAssignee();

// db.Exec or db.ExecContext
CxList databasePkgDbReferences = All.FindAllReferences(databasePkgAssignees);
CxList databasePkgDbMembers = databasePkgDbReferences.GetMembersOfTarget();

CxList databasePkgResultAssignees = databasePkgDbMembers.FindByShortName("Exec*").GetFathers().GetAssignee();
CxList databasePkgResultReferences = All.FindAllReferences(databasePkgResultAssignees);
// Add all Result type members.
externalMethods.Add(databasePkgResultReferences.GetMembersOfTarget());


// https://golang.org/pkg/time/
List<string> timePkgTypeMethods = new List<string>{
	"After*", "Date", "FixedZone", "LoadLocation", "New*", 
	"Now", "Parse*", "Since", "Tick", "Unix", "Until"
	};

CxList timePkgTypeMembers = All.FindByMemberAccess("\"time\".*").FindByShortNames(timePkgTypeMethods);
externalMethods.Add(timePkgTypeMembers);

CxList timePkgDurationType = numericReferences.FindByType("Duration");
CxList timePkgDurationMembers = timePkgDurationType.GetMembersOfTarget();
List<string> timePkgDurationMethods = new List<string>(){
		"Hours", "Minutes", "Nanoseconds", "Seconds"
		};
externalMethods.Add(timePkgDurationMembers.FindByShortNames(timePkgDurationMethods));

CxList timePkgTimeType = numericReferences.FindByType("Time");
CxList timePkgTimeMembers = timePkgTimeType.GetMembersOfTarget();
List<string> timePkgTimeMethods = new List<string>(){
		"Add*", "After", "Before", "Day", "Equal", "Hour", "ISOWeek",
		"In", "IsZero", "Local", "Location", "Minute", "Month", "Nanosecond",
		"Round", "Second", "Sub", "Truncate", "UTC", "Unix*", "Weekday", "Year*"
		};
externalMethods.Add(timePkgTimeMembers.FindByShortNames(timePkgTimeMethods));

CxList timePkgTimerType = numericReferences.FindByType("Timer");
CxList timePkgTimerMembers = timePkgTimerType.GetMembersOfTarget();
List<string> timePkgTimerMethods = new List<string>(){
		"Reset", "Stop"
		};
externalMethods.Add(timePkgTimerMembers.FindByShortNames(timePkgTimerMethods));

// Operators
CxList unaryExpr = Find_Unarys();
CxList binaryExpr = Find_BinaryExpr();

List<string> binaryOperators = new List<string>(){
		"==", "!=", "<", ">", "<=", ">=", 		// Relational Operators
		"||", "&&",								// Logical Operators
		"<<", ">>",	"/", "^", "&"				// Bitwise Operators
		};
CxList operators = binaryExpr.FindByShortNames(binaryOperators);

List<string> unaryOperators = new List<string>(){
		"!", 	// Not
		"*", 	// Pointer (eg: *a)
		"&", 	// Address (eg: &a)
		"++",	// Increment
		"--",	// Decrement
		"&^"	// get the bits that are in 'left' AND NOT 'right'
		};
operators.Add(unaryExpr.FindByShortNames(unaryOperators));

CxList nonSanitizer = Find_AssignExpr().GetByAncs(operators);
operators -= nonSanitizer.GetAncOfType(typeof(BinaryExpr));


// Add to result
result = numericReferences;
result.Add(typeConversions);
result.Add(constants);
result.Add(internalMethods);
result.Add(externalMethods);
result.Add(operators);

// discard byte slices
CxList sliceDeclarations = Find_Byte_Slices();
CxList sliceInstances = All.FindAllReferences(sliceDeclarations);
result -= sliceInstances;