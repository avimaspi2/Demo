//Find_Required_Null_Termination_String
//-------------------------------------
//  http://en.wikibooks.org/wiki/C_Programming/Strings#The_strcat_function

CxList methods = Find_Methods();

var methodsToSearch = new List<string>{
		"lstrcpy",			
		"lstrlen",			
		"strcat",
		"strchr",			
		"strcmp",			
		"strcoll",					
		"strcspn",			
		"strerror",			
		"strlen",			
		"strpbrk",			
		"strrchr",			
		"strspn",			
		"strstr",			
		"strtok",			
		"strxfrm"
		};

CxList methodsList = methods.FindByShortNames(methodsToSearch);
result.Add(methodsList);