CxList methods = Find_Methods();

List<string> allMethod = new List<string> {
		"bcopy",
		"cuserid",
		"getpwd",
		"getwd",
		"LoadModule",
		"MoveFile",
		"MoveFileEx",
		"RegCreateKey",
		"RegEnumKey",
		"RegOpenKey",
		"RegQueryValue",
		"RegSetValue",
		"sqlite3_get_table"
		};

result = methods.FindByShortNames(allMethod);