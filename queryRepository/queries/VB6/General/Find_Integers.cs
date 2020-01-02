CxList ints = All.FindByType("int") + All.FindByType("int16") + 
	All.FindByType("int32") + All.FindByType("int64") + 
	All.FindByType("long") + All.FindByType("decimal") + 
	All.FindByType("float") + All.FindByType("double");

CxList methods = Find_Methods();
CxList converted = 
	methods.FindByName("Convert.toboolean") +
	methods.FindByName("convert.tobyte") +
	methods.FindByName("convert.tosbyte") +
	methods.FindByName("convert.todatetime") +
	methods.FindByName("convert.toint16") +
	methods.FindByName("convert.toint32") +
	methods.FindByName("convert.toint64") +
	methods.FindByName("convert.todecimal") +
	methods.FindByName("convert.todouble") +
	methods.FindByName("convert.tosingle") +
	methods.FindByName("convert.touint16") +
	methods.FindByName("convert.touint32") +
	methods.FindByName("convert.touint64") + 

	methods.FindByName("int.parse", StringComparison.OrdinalIgnoreCase) + 
	methods.FindByName("int16.parse", StringComparison.OrdinalIgnoreCase) + 
	methods.FindByName("int32.parse", StringComparison.OrdinalIgnoreCase) + 
	methods.FindByName("int64.parse", StringComparison.OrdinalIgnoreCase) + 
	methods.FindByName("long.parse", StringComparison.OrdinalIgnoreCase) + 
	methods.FindByName("decimal.parse", StringComparison.OrdinalIgnoreCase) + 
	methods.FindByName("double.parse", StringComparison.OrdinalIgnoreCase) + 
	methods.FindByName("float.parse", StringComparison.OrdinalIgnoreCase);


CxList byNames = All.FindByShortName("*length*", false) + 
	All.FindByShortName("*index*", false) + 
	All.FindByMemberAccess("request.mapimagecoordinates") +
	All.FindByMemberAccess("request.contentlength") +
	All.FindByMemberAccess("request.totalbytes") + 
	(All.FindByMemberAccess("request.mapimagecoordinates") +
	All.FindByMemberAccess("request.contentlength") +
	All.FindByMemberAccess("request.totalbytes")).GetTargetOfMembers() +
	methods.FindByShortName("len", false) + 
	methods.FindByShortName("InStr", false) + 
	methods.FindByShortName("InStrRev", false) + 
	methods.FindByShortName("Space", false) + 
	methods.FindByShortName("Asc", false) + 
	methods.FindByShortName("Val", false) + 
	Find_Integers_Components();
byNames -= byNames.FindByType(typeof(StringLiteral));

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

CxList Vb6BuiltInCovertMethods = 
	methods.FindByShortName("Cbool", false) + 
	methods.FindByShortName("Cbyte", false) + 
	methods.FindByShortName("Ccur", false) + 
	methods.FindByShortName("Cdate", false) + 
	methods.FindByShortName("Cdec", false) + 
	methods.FindByShortName("CDbl", false) + 
	methods.FindByShortName("Cint", false) + 
	methods.FindByShortName("CLng", false) + 
	methods.FindByShortName("CSng", false);
	

result = ints + converted + byNames + booleanConditions + Vb6BuiltInCovertMethods;