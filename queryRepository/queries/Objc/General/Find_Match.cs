CxList regex = Find_Regex();
CxList methods = Find_Methods();

CxList regexMembers = regex.GetMembersOfTarget();
string[] methodNames = {
	@"numberOfMatchesInString:options:range:",
	@"enumerateMatchesInString:options:range:usingBlock:",
	@"matchesInString:options:range:",
	@"rangeOfFirstMatchInString:options:range:",
	@"firstMatchInString:options:range:", 
	@"replaceMatchesInString:options:range:withTemplate:",	
	@"stringByReplacingMatchesInString:options:range:withTemplate:",	
	//Swift
	"numberOfMatches:options:range:",
	"enumerateMatches:options:range:using:",
	"matches:options:range:",
	"firstMatch:options:range:",
	"replaceMatches:options:range:withTemplate:",
	"stringByReplacingMatches:options:range:withTemplate:"
	};
result = regexMembers.FindByShortNames(new List<string>(methodNames));

// add C & Xcode regex's methods, such as regexec, regwnexec etc.
result.Add(regex.FindByType(typeof(MethodInvokeExpr)));

//Add NSPredicate cases
CxList stringMatch = Find_Strings().FindByShortName("*MATCHES*");

CxList predicates = methods.FindByShortName(@"predicateWithFormat:*");
predicates.Add(methods.FindByShortName(@"init:format:argumentArray:"));
predicates.Add(methods.FindByShortName("NSPredicate:*").FindByParameterName("format", 0));

CxList predParams = All.GetParameters(predicates, 0);

CxList predEvilParams = predParams.InfluencedBy(stringMatch) + predParams * stringMatch;

result.Add(predicates.InfluencedBy(predEvilParams));