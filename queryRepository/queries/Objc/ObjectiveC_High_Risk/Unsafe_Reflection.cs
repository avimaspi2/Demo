CxList inputs = Find_Interactive_Inputs();
CxList sanitized = Find_General_Sanitize();
CxList methods = Find_Methods();
string[] importNames = {
	"NSClassFromString", "NSClassFromString:" /* Import class */,
	"NSSelectorFromString", "NSSelectorFromString:" /* Import selector */,
	"NSProtocolFromString", "NSProtocolFromString:" /* Import protocol */
	};
CxList potentialReflection = methods.FindByShortNames(new List<string>(importNames));

result = potentialReflection.InfluencedByAndNotSanitized(inputs, sanitized);