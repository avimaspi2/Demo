string [] strType = new string [] {
	"int",
	"long",
	"short",
	"float",
	"double",
	"Integer",
	"bool",	
	"Boolean",
	"AtomicInteger",
	"AtomicLong",	
	"BigDecimal",
	"BigInteger",
	//	"Byte",
	"Double",
	"Float",
	"Long",
	"Short",
	"Number"};	
	

CxList ints = All.FindByTypes(strType);

ints.Add(All.FindByType("Byte", true));

ints.Add(All.FindByType("Date"));
ints.Add(All.FindByType("LocalDateTime"));
ints.Add(All.FindByMemberAccess("Date.parse"));
ints.Add(All.FindByType("Calendar"));
ints.Add(All.FindByType("Locale"));
ints.Add(All.FindByShortNames(new List<string> {"size","length","intValue","Index*"}, false));
ints.Add(All.FindByShortName("indexOf"));

CxList allCastExpr = base.Find_CastExpr();
CxList castInts = All.NewCxList();

HashSet<string> typeNames = new HashSet<string>(strType);
typeNames.Add("Byte");
foreach(CxList ace in allCastExpr)
{
	CastExpr ce = ace.TryGetCSharpGraph<CastExpr>();
	if(ce != null)
	{
		string typeName = ce.TargetType.TypeName;	
		if (typeNames.Contains(typeName))
		{
			castInts.Add(ce.NodeId, ce);
		}
	}
}


CxList methods = Find_Methods();

CxList allUnMethodsMember = Find_UnknownReference();
allUnMethodsMember.Add(methods);
allUnMethodsMember.Add(Find_MemberAccesses());


castInts = allUnMethodsMember.FindByFathers(castInts);
castInts -= castInts.GetMembersOfTarget().GetTargetOfMembers();


CxList casting = All.NewCxList();
casting.Add(All.FindByName("*int.class.cast", StringComparison.OrdinalIgnoreCase));
casting.Add(All.FindByName("*long.class.cast", StringComparison.OrdinalIgnoreCase));
casting.Add(All.FindByName("*short.Parse", StringComparison.OrdinalIgnoreCase)); 
casting.Add(All.FindByName("*double.class.cast", StringComparison.OrdinalIgnoreCase));
casting.Add(All.FindByName("*boolean.class.cast", StringComparison.OrdinalIgnoreCase));
casting.Add(All.FindByName("*float.class.cast", StringComparison.OrdinalIgnoreCase));

CxList parse = All.NewCxList();
parse.Add(All.FindByMemberAccess("Integer.parse*"));
parse.Add(All.FindByMemberAccess("Byte.parse*"));
parse.Add(All.FindByMemberAccess("Double.parse*"));
parse.Add(All.FindByMemberAccess("Float.parse*"));
parse.Add(All.FindByMemberAccess("Long.parse*"));
parse.Add(All.FindByMemberAccess("Boolean.parse*"));
parse.Add(All.FindByMemberAccess("Short.parse*"));

CxList values = All.NewCxList();
values.Add(All.FindByName("byteValue"));
values.Add(All.FindByName("doubleValue"));
values.Add(All.FindByName("floatValue"));
values.Add(All.FindByName("intValue"));
values.Add(All.FindByName("longValue"));
values.Add(All.FindByName("shortValue"));
values.Add(All.FindByName("booleanValue"));
values.Add(All.FindByName("hashCode"));

CxList valueOf = All.NewCxList();
valueOf.Add(All.FindByMemberAccess("Integer.valueOf*"));
valueOf.Add(All.FindByMemberAccess("Byte.valueOf*"));
valueOf.Add(All.FindByMemberAccess("Double.valueOf*"));
valueOf.Add(All.FindByMemberAccess("Float.valueOf*"));
valueOf.Add(All.FindByMemberAccess("Long.valueOf*"));
valueOf.Add(All.FindByMemberAccess("Boolean.valueOf*"));
valueOf.Add(All.FindByMemberAccess("Short.valueOf*"));

CxList eq = All.NewCxList();
eq.Add(All.FindByMemberAccess("Integer.equals"));
eq.Add(All.FindByMemberAccess("Byte.equals"));
eq.Add(All.FindByMemberAccess("Double.equals"));
eq.Add(All.FindByMemberAccess("Float.equals"));
eq.Add(All.FindByMemberAccess("Long.equals"));
eq.Add(All.FindByMemberAccess("Boolean.equals"));
eq.Add(All.FindByMemberAccess("Short.equals"));

CxList decode = All.NewCxList();
decode.Add(All.FindByMemberAccess("Integer.decode"));
decode.Add(All.FindByMemberAccess("Byte.decode"));
decode.Add(All.FindByMemberAccess("Long.decode"));
decode.Add(All.FindByMemberAccess("Short.decode"));

CxList gett = All.NewCxList();
gett.Add(All.FindByMemberAccess("Integer.getInteger"));
gett.Add(All.FindByMemberAccess("Long.getLong"));

CxList binary = base.Find_BinaryExpr();
CxList booleanConditions = binary.FindByShortNames(new List<string>  {
		"<",
		">",
		"==",
		"!=",
		"<>",
		"<=",
		">=",
		"||",
		"&&"});

booleanConditions.Add(base.Find_Unarys().FindByShortName("Not"));

CxList nonSanitizer = Find_AssignExpr().GetByAncs(booleanConditions);
booleanConditions -= nonSanitizer.GetAncOfType(typeof(BinaryExpr));

CxList methodInts = All.NewCxList();
methodInts.Add(All.FindByReturnType("int"));
methodInts.Add(All.FindByReturnType("bool"));
methodInts.Add(All.FindByReturnType("long")); 
methodInts.Add(All.FindByReturnType("short"));
methodInts.Add(All.FindByReturnType("float"));
methodInts.Add(All.FindByReturnType("double"));
methodInts.Add(All.FindByReturnType("Integer"));

methodInts = methods.FindAllReferences(methodInts);

CxList stringMembers = methods.FindByMemberAccess("String.*");
CxList stringIntegers = stringMembers.FindByShortNames(new List<string>{
		"compareTo*", "contains", "contentEquals", 
		"endsWith", "equals*", "hashCode",
		"indexOf", "lastIndexOf", "length",
		"matches", "regionMatches", "startsWith"});
stringIntegers.Add(All.FindByMemberAccess("*.length")); // also array length, not only string

CxList otherIntegers = All.FindByMemberAccess("System.in");
otherIntegers.Add(All.FindByMemberAccess("*.getInputStream"));

otherIntegers = otherIntegers.GetMembersOfTarget() * methods;

CxList numberAndDecimal = All.FindByType("DecimalFormat");
numberAndDecimal.Add(All.FindByType("NumberFormat"));

CxList resultSetMembers = methods.FindByMemberAccess("ResultSet.*");
CxList resultSet = resultSetMembers.FindByShortNames(new List<string>{
		"absolute", "findColumn", "first", "getBigDecimal",
		"getBoolean", "getByte", "getBytes", "getConcurrency",
		"getDate", "getDouble", "getFetch*", "getFloat", "getInt",
		"getLong", "getRow", "getShort", "getTime", "getTimeStamp",
		"getType", "is*", "last", "next", "previous", "row*", "wasNull"});

CxList getESAPI = Get_ESAPI();
CxList ESAPIRandom = getESAPI.FindByMemberAccess("Randomizer.getRandom*");
CxList ESAPIValidatorGet = getESAPI.FindByMemberAccess("Validator.getValid*");
CxList ESAPIUserGet = getESAPI.FindByMemberAccess("User.get*");
CxList ESAPISafeRequestGet = getESAPI.FindByMemberAccess("SafeRequest.get*");
CxList ESAPISafeResponsetGet = getESAPI.FindByMemberAccess("SafeResponse.get*");
CxList ESAPIEncryptor = getESAPI.FindByMemberAccess("Encryptor.*");
CxList ESAPISecurity = getESAPI.FindByMemberAccess("SecurityConfiguration.get*");
CxList ESAPI = All.NewCxList();
ESAPI.Add(ESAPIRandom.FindByShortName("*Boolean"));
ESAPI.Add(ESAPIRandom.FindByShortName("*Integer"));
ESAPI.Add(ESAPIRandom.FindByShortName("*Long"));
ESAPI.Add(ESAPIRandom.FindByShortName("*Real"));
ESAPI.Add(ESAPIValidatorGet.FindByShortName("*Date"));
ESAPI.Add(ESAPIValidatorGet.FindByShortName("*Double"));
ESAPI.Add(ESAPIValidatorGet.FindByShortName("*Integer"));
ESAPI.Add(ESAPIValidatorGet.FindByShortName("*Number"));
ESAPI.Add(ESAPIUserGet.FindByShortName("*Id"));
ESAPI.Add(ESAPIUserGet.FindByShortName("*Time"));
ESAPI.Add(ESAPIUserGet.FindByShortName("*Count"));
ESAPI.Add(getESAPI.FindByMemberAccess("Validator.isValid*"));
ESAPI.Add(getESAPI.FindByMemberAccess("User.is*"));
ESAPI.Add(getESAPI.FindByMemberAccess("User.verifyPassword"));
ESAPI.Add(ESAPISafeRequestGet.FindByShortName("*Length"));
ESAPI.Add(ESAPISafeRequestGet.FindByShortName("*DateHeader"));
ESAPI.Add(ESAPISafeRequestGet.FindByShortName("*getIntHeader"));
ESAPI.Add(ESAPISafeRequestGet.FindByShortName("*Port"));
ESAPI.Add(ESAPISafeResponsetGet.FindByShortName("*containsHeader"));
ESAPI.Add(ESAPISafeResponsetGet.FindByShortName("*getBufferSize"));
ESAPI.Add(ESAPISafeResponsetGet.FindByShortName("*isCommitted"));
ESAPI.Add(ESAPISafeResponsetGet.FindByShortName("*getBufferSize"));
ESAPI.Add(getESAPI.FindByMemberAccess("AccessController.getBufferSize"));
ESAPI.Add(ESAPISafeResponsetGet.FindByShortName("getBufferSize.is*"));
ESAPI.Add(getESAPI.FindByMemberAccess("ValidationErrorList.isEmpty"));
ESAPI.Add(getESAPI.FindByMemberAccess("ValidationErrorList.size"));
ESAPI.Add(getESAPI.FindByMemberAccess("Logger.id*"));
ESAPI.Add(ESAPIEncryptor.FindByShortName("*Stamp"));
ESAPI.Add(ESAPIEncryptor.FindByShortName("verify*"));
ESAPI.Add(ESAPISecurity.FindByShortName("*AllowedFileUploadSize"));
ESAPI.Add(ESAPISecurity.FindByShortName("*AllowedLoginAttempts"));
ESAPI.Add(ESAPISecurity.FindByShortName("*LogEncodingRequired"));
ESAPI.Add(ESAPISecurity.FindByShortName("*MaxOldPasswordHashes"));
ESAPI.Add(ESAPISecurity.FindByShortName("*RememberTokenDuration"));
ESAPI.Add(ESAPISecurity.FindByShortName("*MaxOldPasswordHashes"));
	

CxList defaultUser = All.FindByType("DefaultUser").GetMembersOfTarget();;
ESAPI.Add(defaultUser.FindByShortNames(new List<string> {
		"getAccountId",
		"*Time",
		"*Count",
		"getLocale",
		"is*",
		"verifyPassword"}));
			
CxList enums = base.Find_EnumMemberDecl().GetAncOfType(typeof(ClassDecl));


//New Java 8 Time api
CxList java8time = All.NewCxList();
java8time.Add(methods.FindByMemberAccess("Clock.millis"));

CxList durationMembers = methods.FindByMemberAccess("Duration.*");
java8time.Add(durationMembers.FindByShortNames(new List<string>{
		"get", "getNano", "getSeconds", "isNegative", "isZero", 
		"toDays", "toHours", "toMillis", "toMinutes", "toNanos"}));

CxList instantMembers = methods.FindByMemberAccess("Instant.*");
java8time.Add(instantMembers.FindByShortNames(new List<string>{
		"get*", "is*", "toEpochMilli", "until"}));

CxList localDateMembers = methods.FindByMemberAccess("LocalDate.*");
java8time.Add(localDateMembers.FindByShortNames(new List<string>{
		"get", "getDayOfMonth", "getDayOfYear", "getLong", "getMonthValue", "getYear",
		"is*", "lengthOfMonth", "lengthOfYear", "toEpochDay", "until"}));

CxList localDateTimeMembers = methods.FindByMemberAccess("LocalDateTime.*");
java8time.Add(localDateTimeMembers.FindByShortNames(new List<string>{
		"get", "getDayOfMonth", "getDayOfYear", "getHour", "getLong", "getMinute", 
		"getMonthValue", "getNano", "getSecond", "getYear", "is*", "until"}));

CxList localTimeMembers = methods.FindByMemberAccess("LocalTime.*");
java8time.Add(localTimeMembers.FindByShortNames(new List<string>{
		"get*", "is*", "toNanoOfDay", "toSecondOfDay", "until"}));

CxList monthDayMembers = methods.FindByMemberAccess("MonthDay.*");
java8time.Add(monthDayMembers.FindByShortNames(new List<string>{
		"get", "getDayOfMonth", "getLong", "getMonthValue", "is*"}));

CxList offsetDateTimeMembers = methods.FindByMemberAccess("OffsetDateTime.*");
java8time.Add(offsetDateTimeMembers.FindByShortNames(new List<string>{
		"get", "getDayOfMonth", "getDayOfYear", "getHour", "getLong", 
		"getMinute", "getMonthValue", "getNano", "getSecond", "getYear", 
		"is*", "toEpochSecond", "until"}));

CxList offsetTimeMembers = methods.FindByMemberAccess("OffsetTime.*");
java8time.Add(offsetTimeMembers.FindByShortNames(new List<string>{
		"get", "getHour", "getLong", "getMinute", 
		"getNano", "getSecond", "is*", "until"}));

CxList periodMembers = methods.FindByMemberAccess("Period.*");
java8time.Add(periodMembers.FindByShortNames(new List<string>{
		"get", "getDays", "getMonths", "getYears", "is*", "toTotalMonths"}));

CxList yearMembers = methods.FindByMemberAccess("Year.*");
java8time.Add(yearMembers.FindByShortNames(new List<string>{
		"get*", "is*", "length", "until"}));

CxList yearMonthMembers = methods.FindByMemberAccess("YearMonth.*");
java8time.Add(yearMonthMembers.FindByShortNames(new List<string>{
		"get", "getLong", "getMonthValue", "getYear", 
		"is*", "lengthOfMonth", "lengthOfYear", "until"}));

CxList zonedDateTimeMembers = methods.FindByMemberAccess("ZonedDateTime.*");
java8time.Add(zonedDateTimeMembers.FindByShortNames(new List<string>{
		"get", "getDayOfMonth", "getDayOfYear", "getHour", 
		"getLong", "getMinute", "getMonthValue", "getNano", 
		"getSecond", "getYear", "isSupported", "until"}));

CxList zoneOffsetMembers = methods.FindByMemberAccess("ZoneOffset.*");
java8time.Add(zoneOffsetMembers.FindByShortNames(new List<string>{
		"get", "getLong", "getTotalSeconds", "isSupported"}));

CxList dayOfWeekMembers = methods.FindByMemberAccess("DayOfWeek.*");
java8time.Add(dayOfWeekMembers.FindByShortNames(new List<string>{
		"get", "getLong", "getValue", "isSupported"}));

CxList monthMembers = methods.FindByMemberAccess("Month.*");
java8time.Add(monthMembers.FindByShortNames(new List<string>{
		"firstDayOfYear", "get", "getLong", "getValue", 
		"isSupported", "length", "maxLength", "minLength"}));

// Add int input methods
CxList scanner = All.FindByMemberAccess("Scanner.*");

CxList intInputs = All.NewCxList();
intInputs.Add(scanner.FindByShortName("nextBigDecimal"));
intInputs.Add(scanner.FindByShortName("nextBigInteger"));
intInputs.Add(scanner.FindByShortName("nextBoolean"));
intInputs.Add(scanner.FindByShortName("nextByte"));
intInputs.Add(scanner.FindByShortName("nextDouble"));
intInputs.Add(scanner.FindByShortName("nextFloat"));
intInputs.Add(scanner.FindByShortName("nextInt"));
intInputs.Add(scanner.FindByShortName("nextLong")); 	
intInputs.Add(scanner.FindByShortName("nextShort")); 

result = ints;
result.Add(intInputs,
	casting,
	parse,
	values,
	gett,
	booleanConditions,
	stringIntegers,
	valueOf,
	eq,
	decode,
	otherIntegers,
	numberAndDecimal,
	resultSet,
	ESAPI,
	castInts);
result.Add(All.FindByType(enums));
result.Add(java8time, methodInts);

//Add Collection: List, HashSet, SetView, NavigableSet, SortedSet, TreeSet
CxList genericCollections = All.FindByTypes(new string[] {
	"Arraylist", "List", "HashSet", "*SetView", 
	"NavigableSet", "SortedSet", "TreeSet"});

CxList genericTypeRef = base.Find_GenericTypeRefs();

CxList genericDeclarators = genericCollections.FindByType(typeof(Declarator));
CxList genericTypes = genericTypeRef.FindByFathers(genericCollections) * ints;
CxList genericVariableDecl = genericTypes.GetFathers().GetFathers();
CxList genericIntDecl = genericDeclarators.GetByAncs(genericVariableDecl);

CxList methodDecls = Find_MethodDeclaration();
CxList genericTypeMethods = genericTypeRef.FindByFathers(methodDecls);
CxList intMethods = genericTypeRef.GetByAncs(genericTypeMethods) * ints;
genericIntDecl.Add(intMethods.GetFathers().GetFathers());

result.Add(All.FindAllReferences(genericIntDecl));