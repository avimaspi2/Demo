//Find all methods
CxList methods = Find_Methods();

//Find Locale senstive funtions
CxList localeSensitiveMethods = methods.FindByShortNames(new List<string>{
		"toLowerCase",
		"toUpperCase"});

//Find all parameters used on the Locale sensitve funtions
CxList localeSensitiveMethodsParams = All.GetParameters(localeSensitiveMethods);

//Find Locale sensitive functions that use the sanitizer parameter "Locale.ROOT"
CxList sanitizedMethods = localeSensitiveMethodsParams.FindByName("Locale.ROOT", true)
													.GetAncOfType(typeof(MethodInvokeExpr));

//Remove the sanitized funtions from the list
localeSensitiveMethods -= sanitizedMethods;

//Find String search or comparison methods
CxList targetMethods = methods.FindByShortNames(new List<string>{
		"compareTo*",
		"contains",
		"contentEquals",
		"endsWith",
		"equals*",
		"*indexOf",
		"matches",
		"startsWith",
		"join",
		"replace"}, false);

//Adds all methods from the class Strings - Java and Google Guava API
targetMethods.Add(All.FindByMemberAccess("Strings", "*"));
//Adds all methods from the class StringUtils - Apache Commons Lang3 API
targetMethods.Add(All.FindByMemberAccess("StringUtils", "*"));

result = targetMethods.DataInfluencedBy(localeSensitiveMethods);
//
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);