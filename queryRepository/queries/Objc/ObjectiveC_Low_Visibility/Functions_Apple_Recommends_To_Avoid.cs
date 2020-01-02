CxList methods = Find_Methods();

List<string> methodsNames = new List<string> {"fopen", "chown","chmod","stat", "mktemp",		
		// Taken from Apple Security Coding Guide document Table 2-1 "String functions to use and avoid"
		"strcat","strcpy","strncat","strncpy","sprintf", "vsprintf","gets"
		};

CxList res = methods.FindByShortNames(methodsNames);

result = res;