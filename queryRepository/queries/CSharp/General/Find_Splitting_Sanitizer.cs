CxList strings = Find_Strings();
CxList chars = All.FindByType(typeof(CharLiteral));
CxList methods = Find_Methods();

// All replaces that contain \r or \n as first parameter should be removed
CxList replace = methods.FindByShortName("Replace*");
CxList replaceEnter = strings.GetParameters(replace, 0);
replaceEnter.FindByShortNames(new List<string> {
		@"*\n*", @"*\r*", @"*%0a*", @"*%0d*"});
replace = replace.FindByParameters(replaceEnter);

// Also all places that leave only the substring before the "indexOf" \r or \n are relevant sanitizers
CxList substring = methods.FindByMemberAccess("*.Substring");
CxList indexof = methods.FindByMemberAccess("*.IndexOf");
CxList indexOfParam = chars.GetParameters(indexof);
indexOfParam = indexOfParam.FindByRegex(@"'\\[rn]'") + 
	indexOfParam.FindByShortNames(new List<string> {@"*%0a*",@"*%0d*"});

indexof = indexof.FindByParameters(indexOfParam);
CxList substringed = substring.DataInfluencedBy(indexof).GetTargetOfMembers();

// Microsoft Core Header Sanitizer
CxList mscore = methods.FindByMemberAccess("HttpHeaders.Add");
mscore.Add(methods.FindByMemberAccess("HttpResponseHeaders.Add"));
mscore.Add(methods.FindByMemberAccess("HttpResponse.AddHeader"));
mscore.Add(methods.FindByMemberAccess("HttpResponse.AppendCookie"));
mscore.Add(methods.FindByMemberAccess("HttpResponse.AppendHeader"));
mscore.Add(methods.FindByMemberAccess("HttpCookieCollection.Add"));

// ESAPI Headers
CxList esapi = methods.FindByMemberAccess("HTTPUtilities.AddCsrfToken");

result = Find_Integers();
result.Add(replace);
result.Add(substringed);
result.Add(mscore);
result.Add(esapi);
result.Add(All.FindAllReferences(substringed).DataInfluencedBy(substringed));