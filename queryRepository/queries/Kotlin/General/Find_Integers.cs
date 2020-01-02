CxList binary = Find_BinaryExpr();
CxList allCastExpr = Find_CastExpr();
CxList assignExpr = Find_AssignExpr();
CxList unarysExpr = Find_Unarys();
CxList memberAccesses = Find_MemberAccesses();
CxList enumMemberDecl = Find_EnumMemberDecl();

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
	"Number",
	//unsigned
	"UInt",
	"ULong",
	"UByte"};	
	
CxList ints = All.FindByTypes(strType);
ints.Add(All.FindByType("Byte", true));

string [] typesNames = new string []{"Date","Calendar","Locale"};
ints.Add(All.FindByTypes(typesNames));

ints.Add(All.FindByMemberAccess("Date.parse"));

ints.Add(All.FindByShortNames(new List<string> {"size","length","intValue","Index*"}, false));
ints.Add(All.FindByShortName("indexOf"));



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

CxList unknowRefMethdodsTypes = Find_UnknownReference();
unknowRefMethdodsTypes.Add(methods);
unknowRefMethdodsTypes.Add(memberAccesses);

castInts = unknowRefMethdodsTypes.FindByFathers(castInts);
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
List<string> namesTypesAll = new List<string>  {
		"byteValue","doubleValue","floatValue","intValue",
		"longValue", "shortValue","booleanValue","hashCode" };

values.Add(All.FindByShortNames(namesTypesAll));


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
booleanConditions.Add(unarysExpr.FindByShortName("Not"));
CxList nonSanitizer = assignExpr.GetByAncs(booleanConditions);
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
CxList numberAndDecimal = All.FindByTypes(new string []{"DecimalFormat","NumberFormat"});

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

// ESAPI
CxList ESAPI = All.NewCxList();

List<string> ESAPIRandomNames = new List<string>  {
		"*Boolean","*Integer",
		"*Long", "*Real" };

ESAPI.Add(ESAPIRandom.FindByShortNames(ESAPIRandomNames));

List<string> ESAPIValidatorGetNames = new List<string>  {
		"*Date","*Double",
		"*Integer", "*Number" };

ESAPI.Add(ESAPIValidatorGet.FindByShortNames(ESAPIValidatorGetNames));

List<string> ESAPIUserGetNames = new List<string>  {
		"*Id","*Time",
		"*Count" };

ESAPI.Add(ESAPIUserGet.FindByShortNames(ESAPIUserGetNames));

ESAPI.Add(getESAPI.FindByMemberAccess("Validator.isValid*"));
ESAPI.Add(getESAPI.FindByMemberAccess("User.is*"));
ESAPI.Add(getESAPI.FindByMemberAccess("User.verifyPassword"));
ESAPI.Add(getESAPI.FindByMemberAccess("AccessController.getBufferSize"));
ESAPI.Add(getESAPI.FindByMemberAccess("ValidationErrorList.isEmpty"));
ESAPI.Add(getESAPI.FindByMemberAccess("ValidationErrorList.size"));
ESAPI.Add(getESAPI.FindByMemberAccess("Logger.id*"));

List<string> ESAPISafeRequestGetNames = new List<string>  {
		"*Length","*DateHeader","*getIntHeader","*Port" };

ESAPI.Add(ESAPISafeRequestGet.FindByShortNames(ESAPISafeRequestGetNames));

List<string> ESAPISafeResponsetGetNames = new List<string>  {
		"*containsHeader","*getBufferSize","*isCommitted","*getBufferSize","getBufferSize.is*" };

ESAPI.Add(ESAPISafeResponsetGet.FindByShortNames(ESAPISafeResponsetGetNames));

List<string> ESAPIEncryptorNames = new List<string>  {
		"*Stamp","verify*" };

ESAPI.Add(ESAPIEncryptor.FindByShortNames(ESAPIEncryptorNames));

List<string> ESAPISecurityNames = new List<string>  {
		"*AllowedFileUploadSize","*AllowedLoginAttempts",
		"*LogEncodingRequired", "*MaxOldPasswordHashes",
		"*RememberTokenDuration","*MaxOldPasswordHashes" };

ESAPI.Add(ESAPISecurity.FindByShortNames(ESAPISecurityNames));

	
CxList defaultUser = All.FindByType("DefaultUser").GetMembersOfTarget();;
ESAPI.Add(defaultUser.FindByShortNames(new List<string> {
		"getAccountId",
		"*Time",
		"*Count",
		"getLocale",
		"is*",
		"verifyPassword"}));
			
CxList enums = enumMemberDecl.GetAncOfType(typeof(ClassDecl));

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
List<string> typesNamesNumbers = new List<string>  {
		"nextBigDecimal","nextBigInteger",
		"nextBoolean","nextByte","nextDouble",
		"nextFloat","nextInt",
		"nextLong","nextShort"};
intInputs.Add(scanner.FindByShortNames(typesNamesNumbers));

result = ints;
result.Add(intInputs);
result.Add(casting);
result.Add(parse);
result.Add(values);
result.Add(gett);
result.Add(booleanConditions);
result.Add(stringIntegers);
result.Add(valueOf);
result.Add(eq);
result.Add(decode);
result.Add(otherIntegers);
result.Add(numberAndDecimal);
result.Add(resultSet);
result.Add(ESAPI);
result.Add(castInts);
result.Add(All.FindByType(enums));
result.Add(java8time);
result.Add(methodInts);

//Add Collection: List, HashSet, SetView, NavigableSet, SortedSet, TreeSet
CxList genericCollections = All.FindByTypes(new string[] {
	"Arraylist", "List", "HashSet", "*SetView", 
	"NavigableSet", "SortedSet", "TreeSet"});
CxList genericDeclarators = genericCollections.FindByType(typeof(Declarator));
CxList genericTypes = All.FindByFathers(genericCollections).FindByType(typeof(GenericTypeRef)) * ints;
CxList genericVariableDecl = genericTypes.GetFathers().GetFathers();
CxList genericIntDecl = genericDeclarators.GetByAncs(genericVariableDecl);
result.Add(All.FindAllReferences(genericIntDecl));