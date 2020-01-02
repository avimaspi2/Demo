CxList methods = Find_Methods();
CxList sanitizers = Find_Integers();
sanitizers.Add(Find_Encrypt());
sanitizers.Add(Find_ESAPI_Sanitizer());

// Methods 'filter_input' and 'filter_var' can be sanitizers if called with some filters.
CxList filter_inputs = methods.FindByShortNames(new List<string>(new string[] {"filter_input","filter_var"}));
CxList filter_names = All.GetParameters(filter_inputs).FindByType(typeof(Param));
// Find only the FILTERS considered sanitizers
filter_names = filter_names.FindByShortNames(new List<string>(new string[] {
	"FILTER_VALIDATE_*","FILTER_SANITIZE_ENCODED","FILTER_SANITIZE_NUMBER_FLOAT",
	"FILTER_SANITIZE_NUMBER_INT","FILTER_SANITIZE_SPECIAL_CHARS","FILTER_SANITIZE_FULL_SPECIAL_CHARS",
	"FILTER_SANITIZE_STRING","FILTER_SANITIZE_STRIPPED"}));

sanitizers.Add(filter_names.GetAncOfType(typeof(MethodInvokeExpr)));

result = sanitizers;