CxList methods = Find_Methods();
CxList integerTypes = 
	All.FindByType("binary_integer") +
	All.FindByType("natural") +
	All.FindByType("naturaln") + 
	All.FindByType("positive") + 
	All.FindByType("positiven") + 
	All.FindByType("signtype") + 
	All.FindByType("simple_integer") + 
	All.FindByType("binary_float") +  
	All.FindByType("binary_double") + 
	All.FindByType("dec") +
	All.FindByType("decimal") +
	All.FindByType("double") +
	All.FindByType("float") +
	All.FindByType("integer") +
	All.FindByType("int") +
	All.FindByType("numeric") +
	All.FindByType("number") +
	All.FindByType("real") +
	All.FindByType("smallint") +
	All.FindByType("pls_integer") +
	All.FindByType("boolean") +
	All.FindByType("bool");

CxList convert = 
	methods.FindByShortName("to_number") + 
	All.FindByMemberAccess("UTL_RAW.CAST_TO_BINARY_DOUBLE", false) + 
	All.FindByMemberAccess("UTL_RAW.CAST_TO_BINARY_FLOAT", false) + 
	All.FindByMemberAccess("UTL_RAW.CAST_TO_BINARY_INTEGER", false) + 
	All.FindByMemberAccess("UTL_RAW.CAST_TO_NUMBER", false) + 
	All.FindByMemberAccess("UTL_RAW.LENGTH", false) + 
	All.FindByMemberAccess("DBMS_UTILITY.SQLID_TO_SQLHASH", false) + 
	All.FindByMemberAccess("DBMS_UTILITY.GET_HASH_VALUE", false);


result = integerTypes + convert;