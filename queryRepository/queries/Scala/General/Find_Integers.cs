string [] strType = new string [] {
	"int",
	"long",
	"short",
	"float",
	"double",
	"integer",
	"boolean",
	"byte",
	"number",
	"Date",
	"Calendar",
	"locale",
	"BigDecimal",
	"BigInteger",
	"AtomicLong",
	"AtomicBoolean"};	
	

CxList ints = 
	All.FindByTypes(strType, true);

ints.Add(All.FindByShortNames(new List<string> {"size","length","intValue","Index*"}, false));
ints.Add(All.FindByShortName("indexOf"));

CxList allCastExpr = All.FindByType(typeof(CastExpr));
CxList castInts = All.NewCxList();

HashSet<string> typeNames = new HashSet<string>(strType);
typeNames.Add("Byte");
foreach(CxList ace in allCastExpr)
{
	CastExpr ce = ace.data.GetByIndex(0) as CastExpr;
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
castInts = (Find_UnknownReference() + methods + All.FindByType(typeof(MemberAccess))).FindByFathers(castInts);
castInts -= castInts.GetMembersOfTarget().GetTargetOfMembers();


CxList casting = All.NewCxList();
casting.Add(All.FindByName("*int.class.cast", StringComparison.OrdinalIgnoreCase));
casting.Add(All.FindByName("*long.class.cast", StringComparison.OrdinalIgnoreCase));
casting.Add(All.FindByName("*short.Parse", StringComparison.OrdinalIgnoreCase)); 
casting.Add(All.FindByName("*double.class.cast", StringComparison.OrdinalIgnoreCase));
casting.Add(All.FindByName("*boolean.class.cast", StringComparison.OrdinalIgnoreCase));
casting.Add(All.FindByName("*float.class.cast", StringComparison.OrdinalIgnoreCase));

CxList parse = All.NewCxList();
parse.Add(All.FindByMemberAccesses(new string[]{
	"Integer.parse*",
	"Byte.parse*",
	"Double.parse*",
	"Float.parse*",
	"Long.parse*",
	"Boolean.parse*",
	"Short.parse*",
	"Date.parse"}));

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
valueOf.Add(All.FindByMemberAccesses(new string[]{
	"Integer.valueOf*",
	"Byte.valueOf*",
	"Double.valueOf*",
	"Float.valueOf*",
	"Long.valueOf*",
	"Boolean.valueOf*",
	"Short.valueOf*"}));

CxList eq = All.NewCxList();
eq.Add(All.FindByMemberAccesses(new string[]{
	"Integer.equals",
	"Byte.equals",
	"Double.equals",
	"Float.equals",
	"Long.equals",
	"Boolean.equals",
	"Short.equals"}));

CxList decode = All.NewCxList();
decode.Add(All.FindByMemberAccesses(new string[]{
	"Integer.decode",
	"Byte.decode",
	"Long.decode",
	"Short.decode"}));

CxList gett = All.NewCxList();
gett.Add(All.FindByMemberAccess("Integer.getInteger"));
gett.Add(All.FindByMemberAccess("Long.getLong"));

CxList binary = All.FindByType(typeof(BinaryExpr));
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

booleanConditions.Add(All.FindByType(typeof(UnaryExpr)).FindByShortName("Not"));

CxList nonSanitizer = All.FindByType(typeof(AssignExpr)).GetByAncs(booleanConditions);
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

CxList stringIntegers = All.NewCxList();
stringIntegers.Add(methods.FindByMemberAccesses(new string[]{
	"String.compareTo*",
	"String.contains",
	"String.contentEquals",
	"String.endsWith",
	"String.equals*",
	"String.hashCode",
	"String.indexOf",
	"String.lastIndexOf",
	"String.length"}));
stringIntegers.Add(All.FindByMemberAccess("*.length")); // also array length, not only string
stringIntegers.Add(methods.FindByMemberAccess("String.matches"));
stringIntegers.Add(methods.FindByMemberAccess("String.regionMatches"));
stringIntegers.Add(methods.FindByMemberAccess("String.startsWith"));

CxList otherIntegers = All.FindByMemberAccess("System.in")
	+ All.FindByMemberAccess("*.getInputStream");
otherIntegers = otherIntegers.GetMembersOfTarget() * methods;

CxList numberAndDecimal = All.FindByType("DecimalFormat") + All.FindByType("NumberFormat");

CxList resultSet = All.NewCxList();
resultSet.Add(All.FindByMemberAccesses(new string[]{
	"ResultSet.absolute",
	"ResultSet.findColumn",
	"ResultSet.first",
	"ResultSet.getBigDecimal",
	"ResultSet.getBoolean",
	"ResultSet.getByte",
	"ResultSet.getBytes",
	"ResultSet.getConcurrency",
	"ResultSet.getDate",
	"ResultSet.getDouble",
	"ResultSet.getFetch*",
	"ResultSet.getFloat",
	"ResultSet.getInt",
	"ResultSet.getLong",
	"ResultSet.getRow",
	"ResultSet.getShort",
	"ResultSet.getTime",
	"ResultSet.getTimeStemp",
	"ResultSet.getType",
	"ResultSet.is*",
	"ResultSet.last",
	"ResultSet.next",
	"ResultSet.previous",
	"ResultSet.row*",
	"ResultSet.wasNull"}));


CxList getESAPI = Get_ESAPI();
CxList ESAPIRandom = getESAPI.FindByMemberAccess("Randomizer.getRandom*");
CxList ESAPIValidatorGet = getESAPI.FindByMemberAccess("Validator.getValid*");
CxList ESAPIUserGet = getESAPI.FindByMemberAccess("User.get*");
CxList ESAPISafeRequestGet = getESAPI.FindByMemberAccess("SafeRequest.get*");
CxList ESAPISafeResponsetGet = getESAPI.FindByMemberAccess("SafeResponse.get*");
CxList ESAPIEncryptor = getESAPI.FindByMemberAccess("Encryptor.*");
CxList ESAPISecurity = getESAPI.FindByMemberAccess("SecurityConfiguration.get*");
CxList ESAPI = All.NewCxList();
ESAPI.Add(ESAPIRandom.FindByShortNames(new List<string> {"*Boolean", "*Integer", "*Long", "*Real"}));
ESAPI.Add(ESAPIValidatorGet.FindByShortNames(new List<string> {"*Date", "*Double", "*Integer", "*Number"}));
ESAPI.Add(ESAPIUserGet.FindByShortNames(new List<string> {"*Id", "*Time", "*Count", "Validator.isValid*"}));
ESAPI.Add(getESAPI.FindByMemberAccesses(new string[]{"User.is*", "ValUser.verifyPassword"}));
ESAPI.Add(ESAPISafeRequestGet.FindByShortNames(new List<string> {"*Length", "*DateHeader", "*getIntHeader", "*Port"}));
ESAPI.Add(ESAPISafeResponsetGet.FindByShortNames(new List<string> {"*containsHeader", "*getBufferSize", "*isCommitted", "*getBufferSize"}));
ESAPI.Add(getESAPI.FindByMemberAccess("AccessController.getBufferSize"));
ESAPI.Add(ESAPISafeResponsetGet.FindByShortName("getBufferSize.is*"));
ESAPI.Add(getESAPI.FindByMemberAccesses(new string[]{"ValidationErrorList.isEmpty", "ValidationErrorList.size", "Logger.id*"}));
ESAPI.Add(ESAPIEncryptor.FindByShortNames(new List<string>{"*Stamp", "verify*"}));
ESAPI.Add(ESAPISecurity.FindByShortNames(new List<string> {
	"*AllowedFileUploadSize",
	"*AllowedLoginAttempts",
	"*LogEncodingRequired",
	"*MaxOldPasswordHashes",
	"*RememberTokenDuration",
	"*MaxOldPasswordHashes"}));
	

CxList defaultUser = All.FindByType("DefaultUser").GetMembersOfTarget();;
ESAPI.Add(defaultUser.FindByShortNames(new List<string> {
	"getAccountId",
	"*Time",
	"*Count",
	"getLocale",
	"is*",
	"verifyPassword"}));
			
CxList enums = All.FindByType(typeof(EnumMemberDecl)).GetAncOfType(typeof(ClassDecl));

// Find all int-inputs
CxList scanner = All.FindByMemberAccess("Scanner.*");

CxList intInputs = All.NewCxList();
intInputs.Add(scanner.FindByShortNames(new List<string> {
	"nextBigDecimal",
	"nextBigInteger",
	"nextBoolean",
	"nextByte",
	"nextDouble",
	"nextFloat",
	"nextInt",
	"nextLong",
	"nextShort"})); 

result = ints;
result.Add(
	intInputs,
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
	castInts,
	All.FindByType(enums));