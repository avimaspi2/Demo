CxList methods = Find_Methods();
CxList array_map_Methods = methods.FindByShortName("array_map", false);
CxList array_mapFirstParam = All.GetParameters(array_map_Methods, 0).FindByType(typeof(StringLiteral));

List<String> mapFirstParamStrings = new List<String>
	{"round", "ceil", "*doubleval", "strlen", "floatval", "intval",
	"*count*", "*size*", "*length*", "*position*", "crc32", "date", "hypot", "min", 
	"pi", "sqrt", "max", "srand", "*errno", "pow", "absint"};

CxList numberSanitizer = array_mapFirstParam.FindByShortNames(mapFirstParamStrings, false);
/*	array_mapFirstParam.FindByShortName("round", false) + 
	array_mapFirstParam.FindByShortName("ceil", false) + 
	array_mapFirstParam.FindByShortName("doubleval", false) +
	array_mapFirstParam.FindByShortName("strlen", false) + 
	array_mapFirstParam.FindByShortName("floatval", false) +
	array_mapFirstParam.FindByShortName("intval", false) + 
	array_mapFirstParam.FindByShortName("*count*", false) + 
	array_mapFirstParam.FindByShortName("*size*", false) + 
	array_mapFirstParam.FindByShortName("*length*", false) + 
	array_mapFirstParam.FindByShortName("*position*", false) +
	array_mapFirstParam.FindByShortName("crc32", false) +
	array_mapFirstParam.FindByShortName("date", false) +
	array_mapFirstParam.FindByShortName("hypot", false) +
	array_mapFirstParam.FindByShortName("min", false) +
	array_mapFirstParam.FindByShortName("pi", false) +
	array_mapFirstParam.FindByShortName("sqrt", false) +
	array_mapFirstParam.FindByShortName("max", false) +
	array_mapFirstParam.FindByShortName("srand", false) +
	array_mapFirstParam.FindByShortName("*errno", false) +
	array_mapFirstParam.FindByShortName("pow", false) + 
	array_mapFirstParam.FindByShortName("absint", false);
*/

result.Add(numberSanitizer.GetAncOfType(typeof(MethodInvokeExpr)));