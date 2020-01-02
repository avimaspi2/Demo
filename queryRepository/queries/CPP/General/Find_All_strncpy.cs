CxList methods = Find_Methods();

result = 
	methods.FindByShortName("strncpy")
	+ methods.FindByShortName("_strncpy*")
	+ methods.FindByShortName("lstrcpyn")
	+ methods.FindByShortName("_tcsncpy*")
	+ methods.FindByShortName("_mbsnbcpy*")
	+ methods.FindByShortName("_wcsncpy*")
	+ methods.FindByShortName("wcsncpy");