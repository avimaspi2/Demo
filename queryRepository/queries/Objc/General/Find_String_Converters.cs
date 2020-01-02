// Find methods that are used to convert strings into C strings.

CxList methods = Find_Methods();

CxList string_converter_methods = All.NewCxList();

string_converter_methods.Add(methods.FindByShortNames(new List<string>{
		"CFStringGetCStringPtr","CFDataGetBytePtr"}));

string_converter_methods.Add(methods.FindByMemberAccess("NSData.getBytes:"));
string_converter_methods.Add(methods.FindByMemberAccess("NSData.getBytes:length:"));
string_converter_methods.Add(methods.FindByMemberAccess("NSData.getBytes:range:"));

CxList NSStringMembers = methods.FindByMemberAccess("NSString.*");
string_converter_methods.Add(NSStringMembers.FindByShortNames(new List<string>
	{
		"getCString:", "getCharacters:", "getCharacters:range:", "getCString:maxLength:", 
		"getCString:maxLength:range:remainingRange:", "getCString:maxLength:encoding:", 
		"getBytes:maxLength:usedLength:encoding:options:range:remainingRange:",
		"getBytes:maxLength:usedLength:encoding:options:range:remaining:",
		}));

result = string_converter_methods;
result.Add(NSStringMembers.FindByShortName("getBytesmaxLength:*").FindByParameterName("usedLength",2));