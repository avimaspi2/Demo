/// Information_Exposure_Through_Extension
/// Look for sensitive infirmation that is flows to the extension input
CxList personalInfo = Find_Personal_Info(); 
CxList sanitize = Find_General_Sanitize();
CxList methods = Find_Methods();

// NSExtensionContext
List<string> methodNames = new List<string>{
		"completeRequestReturningItems:completionHandler:",
		"completeRequest:returningItems:completionHandler:", // Swift
		"completeRequest:completionHandler:"
		};
CxList output = All.GetByAncs(All.GetParameters(methods.FindByShortNames(methodNames), 0)).FindByType(typeof(UnknownReference));
result = personalInfo.InfluencingOnAndNotSanitized(output, sanitize);