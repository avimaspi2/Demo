if(cxScan.IsFrameworkActive("AngularJS"))
{
	CxList methods = Find_Methods();

	// Find all CxEscapedOutput methods => CxEscapedOutput(output)
	CxList escapedOutputs = methods.FindByShortName("CxEscapedOutput");

	// find AngularJS core functions => angular.isEquals(...)
	List<string> coreFunctionStrings = new List<string>(new string[]{
		"equals",
		"isArray",
		"isDate",
		"isDefined",
		"isElement",
		"isFunction",
		"isNumber",
		"isObject",
		"isString",
		"isUndefined"
		});

	CxList coreFunctions = methods.FindByShortNames(coreFunctionStrings);
	CxList ngCoreFunctions = coreFunctions.GetTargetOfMembers()
		.FindByShortName("angular")
		.GetMembersOfTarget();

	// find sanitize methods => $sanitize(html)
	CxList sanitize = methods.FindByShortName("$sanitize");

	// all sanitized results
	result.Add(escapedOutputs);
	result.Add(ngCoreFunctions);
	result.Add(sanitize);
}