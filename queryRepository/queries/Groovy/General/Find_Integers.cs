CxList ints = All.FindByType("int") +
	All.FindByType("long") + All.FindByType("short") + 
	All.FindByType("float") + All.FindByType("double") +  
	All.FindByType("Integer") + All.FindByType("bool") + 
	All.FindByType("Boolean") + All.FindByType("AtomicInteger") +
	All.FindByType("AtomicLong") + All.FindByType("BigDecimal") +
	All.FindByType("BigInteger") + All.FindByType("Byte", true) +
	All.FindByType("Double") + All.FindByType("Float") +
	All.FindByType("Long") +
	All.FindByType("Short") + All.FindByType("Number");

ints.Add(
	All.FindByType("Date") +
	All.FindByMemberAccess("Date.parse") +
	All.FindByType("Calendar") +
	All.FindByType("Locale") +
	All.FindByShortName("size", false) + 
	All.FindByShortName("length", false) + 			
	All.FindByShortName("intValue", false) + 
	All.FindByShortName("Index*", false) +
	All.FindByShortName("indexOf")
	);


CxList allCastExpr = All.FindByType(typeof(CastExpr));
CxList castInts = All.NewCxList();
foreach(CxList ace in allCastExpr)
{
	CastExpr ce = ace.data.GetByIndex(0) as CastExpr;
	if(ce != null)
	{
		string typeName = ce.TargetType.TypeName;		
		if(typeName.Equals("int") || typeName.Equals("long") ||
			typeName.Equals("short") || typeName.Equals("float") ||
			typeName.Equals("double") || typeName.Equals("Integer") || 
			typeName.Equals("bool") || typeName.Equals("Boolean") ||
			typeName.Equals("AtomicInteger") || typeName.Equals("AtomicLong") ||
			typeName.Equals("BigDecimal") || typeName.Equals("BigInteger") ||
			typeName.Equals("Byte") || typeName.Equals("Double") ||
			typeName.Equals("Float") || typeName.Equals("Long") ||
			typeName.Equals("Short") || typeName.Equals("Number"))
		{
			castInts.Add(ce.NodeId, ce);
		}
	}
}


CxList methods = Find_Methods();
castInts = (All.FindByType(typeof(UnknownReference)) + methods + All.FindByType(typeof(MemberAccess))).FindByFathers(castInts);
castInts -= castInts.GetMembersOfTarget().GetTargetOfMembers();


CxList casting = 
	All.FindByName("*int.class.cast", StringComparison.OrdinalIgnoreCase) + 
	All.FindByName("*long.class.cast", StringComparison.OrdinalIgnoreCase) + 
	All.FindByName("*short.Parse", StringComparison.OrdinalIgnoreCase) + 
	All.FindByName("*double.class.cast", StringComparison.OrdinalIgnoreCase) + 
	All.FindByName("*boolean.class.cast", StringComparison.OrdinalIgnoreCase) + 
	All.FindByName("*float.class.cast", StringComparison.OrdinalIgnoreCase);

CxList parse = 
	All.FindByMemberAccess("Integer.parse*") +
	All.FindByMemberAccess("Byte.parse*") +
	All.FindByMemberAccess("Double.parse*") +
	All.FindByMemberAccess("Float.parse*") +
	All.FindByMemberAccess("Long.parse*") +
	All.FindByMemberAccess("Boolean.parse*") +
	All.FindByMemberAccess("Short.parse*");

CxList values = 
	All.FindByName("byteValue") +
	All.FindByName("doubleValue") +
	All.FindByName("floatValue") +
	All.FindByName("intValue") +
	All.FindByName("longValue") +
	All.FindByName("shortValue") +
	All.FindByName("booleanValue") +
	All.FindByName("hashCode");

CxList valueOf =
	All.FindByMemberAccess("Integer.valueOf*") +
	All.FindByMemberAccess("Byte.valueOf*") +
	All.FindByMemberAccess("Double.valueOf*") +
	All.FindByMemberAccess("Float.valueOf*") +
	All.FindByMemberAccess("Long.valueOf*") +
	All.FindByMemberAccess("Boolean.valueOf*") +
	All.FindByMemberAccess("Short.valueOf*");

CxList eq =
	All.FindByMemberAccess("Integer.equals") +
	All.FindByMemberAccess("Byte.equals") +
	All.FindByMemberAccess("Double.equals") +
	All.FindByMemberAccess("Float.equals") +
	All.FindByMemberAccess("Long.equals") +
	All.FindByMemberAccess("Boolean.equals") +
	All.FindByMemberAccess("Short.equals");

CxList decode =
	All.FindByMemberAccess("Integer.decode") +
	All.FindByMemberAccess("Byte.decode") +
	All.FindByMemberAccess("Long.decode") +
	All.FindByMemberAccess("Short.decode");

CxList gett = 
	All.FindByMemberAccess("Integer.getInteger") +
	All.FindByMemberAccess("Long.getLong");

CxList binary = All.FindByType(typeof(BinaryExpr));
CxList booleanConditions =
	binary.FindByShortName("<") +
	binary.FindByShortName(">") +
	binary.FindByShortName("==") +
	binary.FindByShortName("!=") +
	binary.FindByShortName("<>") +
	binary.FindByShortName("<=") +
	binary.FindByShortName(">=") +
	binary.FindByShortName("||") +
	binary.FindByShortName("&&") +
	All.FindByType(typeof(UnaryExpr)).FindByShortName("Not");

CxList nonSanitizer = All.FindByType(typeof(AssignExpr)).GetByAncs(booleanConditions);
booleanConditions -= nonSanitizer.GetAncOfType(typeof(BinaryExpr));

CxList methodInts = All.FindByReturnType("int") + All.FindByReturnType("bool") +
	All.FindByReturnType("long") + All.FindByReturnType("short") + 
	All.FindByReturnType("float") + All.FindByReturnType("double") +  
	All.FindByReturnType("Integer");

methodInts = methods.FindAllReferences(methodInts);

CxList stringIntegers =
	methods.FindByMemberAccess("String.compareTo*") + 
	methods.FindByMemberAccess("String.contains") +
	methods.FindByMemberAccess("String.contentEquals") +
	methods.FindByMemberAccess("String.endsWith") +
	methods.FindByMemberAccess("String.equals*") +
	methods.FindByMemberAccess("String.hashCode") +
	methods.FindByMemberAccess("String.indexOf") +
	methods.FindByMemberAccess("String.lastIndexOf") +
	methods.FindByMemberAccess("String.length") +
	All.FindByMemberAccess("*.length") + // also array length, not only string
	methods.FindByMemberAccess("String.matches") +
	methods.FindByMemberAccess("String.regionMatches") +
	methods.FindByMemberAccess("String.startsWith");

CxList otherIntegers = All.FindByMemberAccess("System.in")
	+ All.FindByMemberAccess("*.getInputStream");
otherIntegers = otherIntegers.GetMembersOfTarget() * methods;

CxList numberAndDecimal = All.FindByType("DecimalFormat") + All.FindByType("NumberFormat");

CxList resultSet = 
	All.FindByMemberAccess("ResultSet.absolute") + 
	All.FindByMemberAccess("ResultSet.findColumn") + 
	All.FindByMemberAccess("ResultSet.first") + 
	All.FindByMemberAccess("ResultSet.getBigDecimal") + 
	All.FindByMemberAccess("ResultSet.getBoolean") + 
	All.FindByMemberAccess("ResultSet.getByte") + 
	All.FindByMemberAccess("ResultSet.getBytes") + 
	All.FindByMemberAccess("ResultSet.getConcurrency") + 
	All.FindByMemberAccess("ResultSet.getDate") + 
	All.FindByMemberAccess("ResultSet.getDouble") + 
	All.FindByMemberAccess("ResultSet.getFetch*") +
	All.FindByMemberAccess("ResultSet.getFloat") + 
	All.FindByMemberAccess("ResultSet.getInt") + 
	All.FindByMemberAccess("ResultSet.getLong") + 
	All.FindByMemberAccess("ResultSet.getRow") + 
	All.FindByMemberAccess("ResultSet.getShort") + 
	All.FindByMemberAccess("ResultSet.getTime") + 
	All.FindByMemberAccess("ResultSet.getTimeStemp") + 
	All.FindByMemberAccess("ResultSet.getType") + 
	All.FindByMemberAccess("ResultSet.is*") + 
	All.FindByMemberAccess("ResultSet.last") + 
	All.FindByMemberAccess("ResultSet.next") + 
	All.FindByMemberAccess("ResultSet.previous") + 
	All.FindByMemberAccess("ResultSet.row*") + 
	All.FindByMemberAccess("ResultSet.wasNull");

CxList ESAPIRandom = Get_ESAPI().FindByMemberAccess("Randomizer.getRandom*");
CxList ESAPIValidatorGet = Get_ESAPI().FindByMemberAccess("Validator.getValid*");
CxList ESAPIUserGet = Get_ESAPI().FindByMemberAccess("User.get*");
CxList ESAPISafeRequestGet = Get_ESAPI().FindByMemberAccess("SafeRequest.get*");
CxList ESAPISafeResponsetGet = Get_ESAPI().FindByMemberAccess("SafeResponse.get*");
CxList ESAPIEncryptor = Get_ESAPI().FindByMemberAccess("Encryptor.*");
CxList ESAPISecurity = Get_ESAPI().FindByMemberAccess("SecurityConfiguration.get*");
CxList ESAPI = 
	ESAPIRandom.FindByShortName("*Boolean") +
	ESAPIRandom.FindByShortName("*Integer") +
	ESAPIRandom.FindByShortName("*Long") +
	ESAPIRandom.FindByShortName("*Real") +
	ESAPIValidatorGet.FindByShortName("*Date") +
	ESAPIValidatorGet.FindByShortName("*Double") +
	ESAPIValidatorGet.FindByShortName("*Integer") +
	ESAPIValidatorGet.FindByShortName("*Number") +
	ESAPIUserGet.FindByShortName("*Id") +
	ESAPIUserGet.FindByShortName("*Time") +
	ESAPIUserGet.FindByShortName("*Count") +
	Get_ESAPI().FindByMemberAccess("Validator.isValid*") +
	Get_ESAPI().FindByMemberAccess("User.is*") +
	Get_ESAPI().FindByMemberAccess("User.verifyPassword") +
	ESAPISafeRequestGet.FindByShortName("*Length") +
	ESAPISafeRequestGet.FindByShortName("*DateHeader") +
	ESAPISafeRequestGet.FindByShortName("*getIntHeader") +
	ESAPISafeRequestGet.FindByShortName("*Port") +
	ESAPISafeResponsetGet.FindByShortName("*containsHeader") +
	ESAPISafeResponsetGet.FindByShortName("*getBufferSize") +
	ESAPISafeResponsetGet.FindByShortName("*isCommitted") +
	ESAPISafeResponsetGet.FindByShortName("*getBufferSize") +
	Get_ESAPI().FindByMemberAccess("AccessController.getBufferSize") +
	ESAPISafeResponsetGet.FindByShortName("getBufferSize.is*") +
	Get_ESAPI().FindByMemberAccess("ValidationErrorList.isEmpty") +
	Get_ESAPI().FindByMemberAccess("ValidationErrorList.size") +
	Get_ESAPI().FindByMemberAccess("Logger.id*") +
	ESAPIEncryptor.FindByShortName("*Stamp") +
	ESAPIEncryptor.FindByShortName("verify*") +
	ESAPISecurity.FindByShortName("*AllowedFileUploadSize") +
	ESAPISecurity.FindByShortName("*AllowedLoginAttempts") +
	ESAPISecurity.FindByShortName("*LogEncodingRequired") +
	ESAPISecurity.FindByShortName("*MaxOldPasswordHashes") +
	ESAPISecurity.FindByShortName("*RememberTokenDuration") +
	ESAPISecurity.FindByShortName("*MaxOldPasswordHashes")
	;

CxList defaultUser = All.FindByType("DefaultUser").GetMembersOfTarget();;
ESAPI.Add(
	defaultUser.FindByShortName("getAccountId") +
	defaultUser.FindByShortName("*Time") +
	defaultUser.FindByShortName("*Count") +
	defaultUser.FindByShortName("getLocale") +
	defaultUser.FindByShortName("is*") +
	defaultUser.FindByShortName("verifyPassword")
	);

CxList enums = All.FindByType(typeof(EnumMemberDecl)).GetAncOfType(typeof(ClassDecl));

result = ints + casting + parse + values + gett + booleanConditions + 
	stringIntegers + valueOf + eq + decode + otherIntegers + numberAndDecimal +
	resultSet + ESAPI + castInts + All.FindByType(enums);