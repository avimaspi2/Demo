// Find unchecked CString conversions
// Check that the length of C string matches the CFStringRef after conversion. 
// Null bytes in the middle of CFStringRef may cause the C string to be shorter.
CxList vulnerableResults = All.NewCxList();
try
{
	CxList methods = Find_Methods();
	CxList unknownRefs = Find_UnknownReference();
	unknownRefs.Add(Find_Param());
	
	CxList cfStringGetCString = methods.FindByShortName("CFStringGetCString");
	cfStringGetCString.Add(methods.FindByShortName("CFStringGetCString:"));
	CxList cfDataGetBytes = methods.FindByShortName("CFDataGetBytes");
	
	CxList stringPtr = methods.FindByShortNames(new List<string>{
			"CFStringGetCStringPtr","CFDataGetBytePtr","CFDataGetBytePtr:"});
	
	CxList nSDataMembers = methods.FindByMemberAccess("NSData.*");
	
	List<string> nSDataMethodsNames = new List<string>{"getBytes:","getBytes:length:","getBytes:range:"};
	
	CxList CStringFirstParam = nSDataMembers.FindByShortNames(nSDataMethodsNames);

	CxList nSStrings = methods.FindByMemberAccess("NSString.*");
	
	List<string> nSStringsMethodsNames = new List<string> {"getCString:","getCharacters:",
			"getCharacters:range:","getCString:maxLength:","getCString:maxLength:range:remainingRange:",
			"getCString:maxLength:encoding:","getCString:maxLength:encoding:",
			"getBytes:maxLength:usedLength:encoding:options:range:remainingRange:",
			"getBytes:maxLength:usedLength:encoding:options:range:remaining:"
			};
	
	CStringFirstParam.Add(nSStrings.FindByShortNames(nSStringsMethodsNames));

	// First parameter of NSString/NSData
	CxList convertedStringsParameter = unknownRefs.GetParameters(CStringFirstParam, 0);
	// Second parameter of CFStringGetCString
	convertedStringsParameter.Add(unknownRefs.GetParameters(cfStringGetCString, 1));
	// Third Parameter of CFDataGetBytes
	convertedStringsParameter.Add(unknownRefs.GetParameters(cfDataGetBytes, 2));

	// return value (Declarator) of CFStringGetCStringPtr
	CxList convertedStringsReturnValue = All.NewCxList();
	convertedStringsReturnValue.Add(stringPtr);
	
	List<string> nSStringMethodsNames = new List<string>{"cStringUsingEncoding:","cString:"};
	
	convertedStringsReturnValue.Add(nSStrings.FindByShortNames(nSStringMethodsNames));

	CxList convertedStrings = All.NewCxList();
	convertedStrings.Add(convertedStringsParameter);
	convertedStrings.Add(convertedStringsReturnValue);

	CxList sanitizers = methods.FindByShortNames(new List<string>{"strlen", "strnlen"});
	// find vulnerable values which are not checked (not sanitized)	
	vulnerableResults = convertedStrings 
		- convertedStrings.DataInfluencingOn(sanitizers).GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
}
catch (Exception error)
{
	cxLog.WriteDebugMessage(error);
}
result = vulnerableResults.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);
result = result.ReduceFlowByPragma();