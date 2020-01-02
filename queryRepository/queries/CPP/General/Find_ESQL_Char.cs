// Searches for Character nodes comming from the ESQL/C code
CxList methods = Find_Methods();
CxList unkownReferences = Find_Unknown_References();

// methods which return char
List<string> charMethods = new List<string>{
		"dececvt", "decfcvt", "ifx_getenv", "ifx_getcur_conn_name"};
result.Add(methods.FindByShortNames(charMethods, false));

// methods which return by reference a char on first parameter
List<string> firstParam = new List<string>{
		"byfill", "rdownshift","rupshift" };
result.Add(unkownReferences.GetParameters(methods.FindByShortNames(firstParam, false), 0));

// methods which return by reference a char on second parameter
List<string> secondParam = new List<string>{
		"biginttoasc","biginttoasc","bycopy","dectoasc","dttoasc","dttofmtasc",
		"ifx_int8toasc","intofmtasc","stcat","stchar"};
result.Add(unkownReferences.GetParameters(methods.FindByShortNames(secondParam, false), 1));

// methods which return by reference a char on third parameter
List<string> thirdParam = new List<string>{
		"ldchar", "rfmtdate", "rfmtdec", "rfmtdouble"};
result.Add(unkownReferences.GetParameters(methods.FindByShortNames(thirdParam, false), 2));

// methods which return by reference a char on fifth parameter
List<string> fifthParam = new List<string>{
		"ifx_dececvt", "ifx_decfcvt"};
result.Add(unkownReferences.GetParameters(methods.FindByShortNames(fifthParam, false), 4));