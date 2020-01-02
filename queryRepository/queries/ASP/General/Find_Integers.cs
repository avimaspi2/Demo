// $ASP*

CxList methods = Find_Methods();
CxList converted = methods.FindByName("convert.toint16") +
					methods.FindByName("convert.toint32") +
					methods.FindByName("convert.toint64") +
					methods.FindByName("convert.todecimal") +
					methods.FindByName("convert.todouble") +
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
					methods.FindByName("float.parse", StringComparison.OrdinalIgnoreCase) +

					// VBScript conversion 
					methods.FindByName("CBool", StringComparison.OrdinalIgnoreCase) +
					methods.FindByName("CByte", StringComparison.OrdinalIgnoreCase) +
					methods.FindByName("CCur", StringComparison.OrdinalIgnoreCase) +
					methods.FindByName("CDec", StringComparison.OrdinalIgnoreCase) +
					methods.FindByName("CDate", StringComparison.OrdinalIgnoreCase) +
					methods.FindByName("CDbl", StringComparison.OrdinalIgnoreCase) +
					methods.FindByName("CInt", StringComparison.OrdinalIgnoreCase) +
					methods.FindByName("CLng", StringComparison.OrdinalIgnoreCase) +
					methods.FindByName("CSng", StringComparison.OrdinalIgnoreCase);


CxList byNames =
	All.FindByShortName("length", false) + 
	methods.FindByShortName("*index*", false) + 
	methods.FindByShortName("strcomp") +
	All.FindByMemberAccess("request.mapimagecoordinates") +
	All.FindByMemberAccess("request.contentlength") +
	All.FindByMemberAccess("request.totalbytes") + 
	(All.FindByMemberAccess("request.mapimagecoordinates") +
	All.FindByMemberAccess("request.contentlength") +
	All.FindByMemberAccess("request.totalbytes")).GetTargetOfMembers() +
			
	// VBScript Len
	All.FindByShortName("len", false);

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


result = converted + byNames + booleanConditions;