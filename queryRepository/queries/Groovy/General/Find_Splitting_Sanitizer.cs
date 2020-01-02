CxList strings = Find_Strings();
CxList chars = All.FindByType(typeof(CharLiteral));
CxList methods = Find_Methods();

// All replaces that contain \r or \n as first parameter should be removed
CxList replace = methods.FindByShortName("replace*");
CxList replaceEnter = strings.GetParameters(replace, 0);
replaceEnter = 
	replaceEnter.FindByShortName(@"*\n*") + 
	replaceEnter.FindByShortName(@"*\r*") +
	replaceEnter.FindByShortName(@"*%0a*") + 
	replaceEnter.FindByShortName(@"*%0d*");
replace = replace.FindByParameters(replaceEnter);

// Also all places that leave only the substring before the "indexOf" \r or \n are relevant sanitizers
CxList substring = methods.FindByMemberAccess("*.substring");
CxList indexof = methods.FindByMemberAccess("*.indexOf");
CxList indexOfParam = chars.GetParameters(indexof);
indexOfParam = 
	indexOfParam.FindByRegex(@"'\\[rn]'") + 
	indexOfParam.FindByShortName(@"*%0a*") + 
	indexOfParam.FindByShortName(@"*%0d*");

indexof = indexof.FindByParameters(indexOfParam);
CxList substringed = substring.DataInfluencedBy(indexof).GetTargetOfMembers();

// ESAPI setHeader
CxList esapi =
	All.FindByMemberAccess("HTTPUtilities.setContentType") +
	All.FindByMemberAccess("HTTPUtilities.safeSendRedirect") +
	All.FindByMemberAccess("HTTPUtilities.safeSetHeader") +
	All.FindByMemberAccess("HTTPUtilities.safeAddCookie");

result = Find_Integers() + replace + substringed + esapi +
	All * All.FindAllReferences(substringed).DataInfluencedBy(substringed);