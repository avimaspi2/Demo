CxList methods = Find_Methods();

//Get methods from old Use_Of_Obsolete Functions:
List<string> obsolete_names = new List<string>(){"_open","_wfopen","_wopen","bcopy","cuserid","fopen","getpwd",
		"getwd","LoadModule","memmove","memset","RegCreateKey","RegEnumKey","RegOpenKey",
		"RegQueryValue","RegSetValue"};

//Obsolescent functions:
List < string > obsolescent_functions = new List<string>(){"asctime","atof","atol","atoll","ctime",
		"fopen","freopen","rewind","setbuf"};

CxList deprecatedFunctions = methods.FindByShortNames(obsolete_names);
deprecatedFunctions.Add(methods.FindByShortNames(obsolescent_functions));

result = deprecatedFunctions;