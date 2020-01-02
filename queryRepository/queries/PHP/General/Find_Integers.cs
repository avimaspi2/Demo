CxList cast = (All.FindByType(typeof(CastExpr)));
CxList NumberCast = All.NewCxList();
HashSet<String> numberNames = new HashSet<string>(){ "int", "integer", "float", "bool", "boolean", "double", "number", "long" };
foreach(CxList l in cast)
{
	CastExpr ce = l.data.GetByIndex(0) as CastExpr;
	string typeName = ce.TargetType.TypeName.ToLower();
	if (numberNames.Contains(ce.TargetType.TypeName.ToLower()))
	{
		NumberCast.Add(
			All.FindByFathers(All.FindById(ce.NodeId)));
	}
}

CxList allMethods = Find_Methods();

List<String> functionStrings = new List<String>();
functionStrings.AddRange(new string[] {"round", "ceil", "doubleval", "strlen", "floatval",
	"intval", "count","size", "length","position", "countof*","sizeof*", "lengthof*","positionof*", 
	"crc32","date", "hypot","min", "pi","sqrt", "max","srand", "*errno","pow",
	//Take care of math functions
	"gmp_*", "variant_*",  "atan", "atan2",  "atanh",  "acos", "acosh",
	"asin",  "asinh",  "tan",  "tan2", "tanh",  "cos",  "cosh",  "sin", 
	"sinh", "exp", "ip2long"	});
CxList numberSanitizer = allMethods.FindByShortNames(functionStrings, false);

numberSanitizer.Add(allMethods.FindByShortName("absint"));

// Constants
CxList variables = All.FindByType(typeof(UnknownReference));
CxList constants = variables.FindByShortNames(new List<string> {
		"GMP_ROUND_ZERO", "GMP_ROUND_PLUSINF", "GMP_ROUND_MINUSINF",
		"GMP_MSW_FIRST", "GMP_LSW_FIRST",
		"GMP_LITTLE_ENDIAN", "GMP_BIG_ENDIAN", "GMP_NATIVE_ENDIAN"});


// Methods that return bool values according to http://php.net/quickref.php
List<String> booleanReturnStrings = new List<String>();
booleanReturnStrings.AddRange(new string[] {"arsort", "asort", "ctype_*", "cubrid_affected_rows", "cubrid_bind",
	"cubrid_close*", "cubrid_col_size", "cubrid_commit", "cubrid_data_seek", "cubrid_disconnect",
	"cubrid_drop", "cubrid_error_code", "cubrid_error_code_facility", "cubrid_field_len",
	"cubrid_free_result", "cubrid_get_autocommit", "cubrid_get_query_timeout", "cubrid_is_instance",
	"cubrid_lob2_bind", "cubrid_load_from_glo", "cubrid_lob2_close", "cubrid_lob2_read", "cubrid_lob2_seek*",
	"cubrid_lob2_size", "cubrid_lob2_tell*", "cubrid_lob_close", "cubrid_lob_send", "cubrid_lock_read",
	"cubrid_lock_write", "cubrid_lob_export", "cubrid_lob2_write", "cubrid_move_cursor", "cubrid_num_*",
	"cubrid_next_result", "cubrid_ping", "cubrid_put", "cubrid_send_glo", "cubrid_save_to_glo", 
	"cubrid_seq_*", "cubrid_set_*", "curl_multi_add_handle", "curl_multi_exec", "curl_multi_remove_handle",
	"curl_multi_select", "curl_setopt", "curl_setopt_array", "cyrus_bind", "cyrus_close", "cyrus_unbind"});


booleanReturnStrings.AddRange(new string[] {"define", "deg2rad", "defined", "dio_seek", "dio_tcsetattr", "dio_write", "dio_truncate", "dl", 
	"empty" });

booleanReturnStrings.AddRange(new string[] {"fclose", "fflush", "feof", "file_put_contents", "fileatime", "filectime", "filegroup", 
	"fileowner", "filemtime", "fileinode", "fileperms", "filepro", 
	"filepro_fieldcount", "filepro_fieldwidth", "filepro_rowcount", "filesize", "filter_has_var", "filter_id", 
	"finfo_close", "finfo_set_flags", "flock", "floor", "fnmatch", "fmod", "fputcsv", "frenchtojd ", "fseek ", 
	"ftell ", "ftok", "func_num_args"});

booleanReturnStrings.AddRange(new string[] {"gc_collect_cycles", "gc_enabled", "get_magic_quotes_gpc", "get_magic_quotes_runtime", 
	"getlastmod", "getmygid", "getmxrr", "getmyinode", "getprotobyname", "getrandmax", "getservbyname", "gmmktime",
	"gnupg_addencryptkey", "gnupg_clearencryptkeys", "gnupg_cleardecryptkeys", "gnupg_adddecryptkey", 
	"gnupg_addsignkey", "gnupg_clearsignkeys", "gnupg_setarmor", "gnupg_setsignmode", "gzseek", "gzeof", "gztell", 
	"gzclose", "gzpassthru", "gzrewind"});

booleanReturnStrings.AddRange(new string[] {"in_array", "import_request_variables", "is_*", "isCloneable", "isInstance", "isInternal", 
	"isSubclassOf", "isDisabled", "isInstantiable", "isIterateable", "isAbstract", "isFinal", "isInterface",
	"isset", "isUserDefined"});

booleanReturnStrings.AddRange(new string[] {"memcache_debug", "memory_get_usage", "memory_get_peak_usage", "method_exists", "mktime", 
	"mkdir", "move_uploaded_file", "mt_getrandmax", "mt_rand", "natsort", "natcasesort", "ord"});

booleanReturnStrings.AddRange(new string[] {"pclose", "pcntl_*", "posix_access", "png2wbmp", 
	"preg_last_error", "proc_close", "proc_nice", "proc_terminate", "property_exists", "putenv", "rad2deg ", "rand", 
	"rename", "rename_function", "restore_error_handler", "restore_exception_handler", "rewind", "rmdir", "rpm_is_valid",
	"rpm_is_valid", "rsort"});

booleanReturnStrings.AddRange(new string[] {"sem_acquire", "sem_remove", "sem_release", "sizeof", "sort", "spl_autoload_register",
	"spl_autoload_unregister", "strcasecmp", "strcoll", "strcspn", "strcmp", "stripos", "strnatcmp", "strrpos",
	"strlen", "strncasecmp", "strpos", "strspn", "strnatcasecmp", "strncmp", "strripos", "strtotime",
	"substr_compare", "substr_count", "symlink"});

booleanReturnStrings.AddRange(new string[] {"tcpwrap_check", "usort", "w32api_deftype", "w32api_register_function", "wddx_add_vars",
	"xattr_set", "xattr_supported", "xattr_remove", "xml_get_current_line_number", "xml_parse_into_struct",
	"xml_parser_free", "xml_set_*", "xml_get_current_byte_index", "xml_get_error_code", 
	"xml_get_current_column_number", "xmlrpc_server_register_method", "xmlrpc_server_register_introspection_callback",
	"xmlrpc_is_fault", "xmlrpc_server_add_introspection_data", "xmlrpc_server_destroy", "xmlrpc_set_type"});

CxList booleanReturnValues = All.NewCxList();
booleanReturnValues = allMethods.FindByShortNames(booleanReturnStrings, false);
// End bool and int value returns

// Add regex matches with only 2 parameters
CxList regexMatches = allMethods.FindByShortNames(new List<string> {"preg_match*", "ereg", "eregi"});

CxList regexWithManyParams = All.GetParameters(regexMatches, 2).GetAncOfType(typeof(MethodInvokeExpr));
regexMatches -= regexWithManyParams;

CxList binaryExpr = All.FindByType(typeof(BinaryExpr));
CxList booleanConditions = binaryExpr.FindByShortNames(new List<string>
	{"<", ">", "==", "!=", "<>","<=", ">=","||","&&","===","!=="}) +
	All.FindByType(typeof(UnaryExpr)).FindByShortName("Not");

//START: Find math operations
//This is a sanitizer because the result of a math operation is always a numeric value
CxList mathOperations = All.NewCxList();
foreach(CxList expr in binaryExpr)
{
	BinaryExpr binaryExpression = expr.TryGetCSharpGraph<BinaryExpr>();
	BinaryOperator op = binaryExpression.Operator;
	if(binaryExpression != null 
		&& (op == BinaryOperator.Add
		|| op == BinaryOperator.Subtract
		|| op == BinaryOperator.Multiply
		|| op == BinaryOperator.Divide
		|| op == BinaryOperator.ShiftRight
		|| op == BinaryOperator.ShiftLeft
		|| op == BinaryOperator.BitwiseOr
		|| op == BinaryOperator.BitwiseAnd
		|| op == BinaryOperator.BitwiseXor
		|| op == BinaryOperator.Modulus))
	{		
		mathOperations.Add(expr);
	}
}
//END: Find math operations

CxList getESAPI = Get_ESAPI();

List<String> ESAPIStrings = new List<String> {"*Attempts", "*Boolean", "*containsHeader", "*Count", "*Date", "*DateHeader",
		"*Double", "*Enabled", "*getBufferSize", "*getIntHeader", "*Id", "*Integer", "*Length",
		"*LogEncodingRequired", "*Long", "*MaxOldPasswordHashes", "*Number", "*Port", "*Real", 
		"*RememberTokenDuration", "*Size", "*Stamp", "*Time", "changeSessionIdentifier", 
		"getDisableIntrusionDetection", "getLogEncodingRequired", "getLogLevel", "getMaxOldPasswordHashes",
		"getQuota", "getRememberTokenDuration", "id*", "is*"};

CxList ESAPI = getESAPI.FindByShortNames(ESAPIStrings);
ESAPI.Add(getESAPI.FindByShortName("Size", false));

ESAPI -= ESAPI.FindByShortName("assert*");

//To retrieve GET/POST request data in joomla and filter according to type(int, float, bool etc)
CxList joomla = allMethods.FindByMemberAccess("JRequest.getInt", false);
joomla.Add(allMethods.FindByMemberAccess("JRequest.getInteger", false));
joomla.Add(allMethods.FindByMemberAccess("JRequest.getFloat", false));
joomla.Add(allMethods.FindByMemberAccess("JRequest.getDouble", false));
joomla.Add(allMethods.FindByMemberAccess("JRequest.getBool", false));
joomla.Add(allMethods.FindByMemberAccess("JRequest.getBoolean", false));

//Add filter_input function  with last parameter "FILTER_VALIDATE_INT" or "FILTER_SANITIZE_NUMBER_INT".
List<string> paramConstNames = new List<string>(new string[]{"FILTER_VALIDATE_INT","FILTER_SANITIZE_NUMBER_INT"});
CxList filterConst = All.FindByShortNames(paramConstNames);
CxList filter = allMethods.FindByShortName("filter_input");
CxList relevant = filterConst.GetParameters(filter, 2);
CxList prospect = filter.FindByParameters(relevant);
CxList sanitInt = prospect + prospect.GetAssignee();

//Add all methods that return integers
CxList methodInts = All.NewCxList();
methodInts.Add(All.FindByReturnType("int"));
methodInts.Add(All.FindByReturnType("bool")); 
methodInts.Add(All.FindByReturnType("float"));
methodInts = allMethods.FindAllReferences(methodInts);

//Date Time Sanitizers
CxList dateTime = Find_ObjectCreations().FindByShortNames(new List<string> {"DateTime", "DateTimeImmutable", "DateInterval", "DatePeriod"}, true);
dateTime.Add(allMethods.FindByShortNames(new List<string> {"date_create", "date_create_from_format", "date_create_immutable", "date_create_immutable_from_format"}, true));

//Add to results
result = NumberCast;
result.Add(sanitInt);
result.Add(numberSanitizer);
result.Add(constants);
result.Add(booleanConditions);
result.Add(mathOperations);
result.Add(ESAPI);
result.Add(booleanReturnValues);
result.Add(regexMatches);
result.Add(Find_Array_Map_Sanitize());
result.Add(joomla);
result.Add(dateTime);
result.Add(methodInts);