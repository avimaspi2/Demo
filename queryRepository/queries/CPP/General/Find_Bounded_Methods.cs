CxList methods = Find_Methods();
result = 
	methods.FindByShortName("memcpy") + 
	methods.FindByShortName("wmemcpy") + 
	methods.FindByShortName("_memccpy") + 

	methods.FindByShortName("memmove") +
	methods.FindByShortName("wmemmove") +

	methods.FindByShortName("memset") +
	methods.FindByShortName("wmemset") +

	methods.FindByShortName("memcmp") +
	methods.FindByShortName("wmemcmp") +

	methods.FindByShortName("memchr") +
	methods.FindByShortName("wmemchr") +

	Find_All_strncpy() + 
	Find_All_strncat();