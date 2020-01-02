/// <summary>
/// Find all methods that return the string length.
/// </summary>
///
CxList methods = Find_Methods();

List<string> strLenMethods = new List<string>{
		// get the length of a string, by using the current locale
		// or a specified locale
		"strlen", "StrLen", "wcslen", "lstrlen", "tcslen", 
		"_mbslen", "_mbslen_l", "_mbstrlen", "_mbstrlen_l", 
		"_tcslen", "_tcsclen", "_tcsclen", "_tcsclen_l",

		// get the length of a string by using the current locale
		// or one that has been passed in
		"strnlen", "strnlen_s", "wcsnlen", "wcsnlen_s",
		"_mbsnlen", "_mbsnlen_l", "_mbstrnlen", "_mbstrnlen_l",
		"mblen"
};

result = methods.FindByShortNames(strLenMethods);