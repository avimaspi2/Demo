CxList methods = Find_Methods();


result = methods.FindByShortNames(new List<string> {"include", "include_once", "require", "require_once", 
	"parsekit_compile_file","php_check_syntax","runkit_import","set_include_path", "virtual"}, false);
/*
result = methods.FindByShortName("include", false) + 
	methods.FindByShortName("include_once", false) +
	methods.FindByShortName("require", false) + 
	methods.FindByShortName("require_once", false) + 
	methods.FindByShortName("parsekit_compile_file", false) + 
	methods.FindByShortName("php_check_syntax", false) + 
	methods.FindByShortName("runkit_import", false) +
	methods.FindByShortName("set_include_path", false) +
	methods.FindByShortName("virtual", false);
*/