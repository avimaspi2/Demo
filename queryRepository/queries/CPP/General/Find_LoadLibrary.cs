CxList methods = Find_Methods();
result = methods.FindByShortNames(new List<string>(){"LoadLibrary","LoadLibraryA","LoadLibraryW","LoadLibraryEx",
		"LoadLibraryExA","LoadLibraryExW","AfxLoadLibrary","LoadModule","dlopen"});