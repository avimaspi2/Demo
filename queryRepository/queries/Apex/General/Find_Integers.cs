string[] types = {"integer", "long", "double", "decimal", "datetime", "date", "boolean", "id"};
CxList byTypes = All.FindByTypes(types);

CxList byNames =
	All.FindByShortName("*Length*", false) + 
	All.FindByShortName("*Index*", false);

// Add VF calls to methods (mostly getters) that return integer
CxList integers = byTypes.FindByFathers(All.FindByType(typeof(ReturnStmt)));
CxList objMethods = Find_Apex_Files().GetMethod(integers);
CxList integersGet = Find_VF_Pages().FindAllReferences(objMethods) - objMethods;

// Add find by return type
CxList methods = Find_Methods();
CxList methodInts = 
	All.FindByReturnType("integer") + 
	All.FindByReturnType("long") + 
	All.FindByReturnType("double") + 
	All.FindByReturnType("decimal") + 
	All.FindByReturnType("datetime") + 
	All.FindByReturnType("date") + 
	All.FindByReturnType("boolean") + 
	All.FindByReturnType("id");
methodInts = methods.FindAllReferences(methodInts);

CxList intValue = All.FindByShortName("intvalue", false) +
	All.FindByShortName("longvalue", false) +
	All.FindByShortName("doublevalue", false) +
	All.FindByShortName("parseint", false);

CxList stringIntegers = methods.FindByMemberAccess("string.compareto") + 
	methods.FindByMemberAccess("string.contains") +
	methods.FindByMemberAccess("string.endswith") +
	methods.FindByMemberAccess("string.equals") +
	methods.FindByMemberAccess("string.startswith");

CxList enums = All.FindByType(typeof(EnumMemberDecl)).GetAncOfType(typeof(ClassDecl));

result = byTypes + byNames + 
	Find_Boolean_Conditions() + 
	integersGet + 
	Find_sObject_Integers() + 
	methodInts + 
	intValue +
	stringIntegers +
	All.FindByType(enums);

result -= result.GetByAncs(All.GetParameters(Find_DB_Input()));