CxList methods = Find_Methods();
List < string > lazyComparison = new List<string> {
	"safecmp","strcmp","wcscmp","_mbscmp","_stricmp","_wcsicmp","_mbsicmp","_tcsicmp","wmemcmp","memcmp","_memicmp",
	"strncmp","_stricmp_l","_wcsicmp_l","_mbsicmp_l","_memicmp_l"
	};
result = methods.FindByShortNames(lazyComparison);