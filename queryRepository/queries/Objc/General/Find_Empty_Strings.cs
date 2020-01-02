CxList strings = Find_Strings();
CxList methods = Find_Methods();

result = strings.FindByShortName("@\"\"");
result.Add(strings.FindByName("\"\""));
//Empty string can be created by 'string', 'init' and 'new' methods of NSString and NSMUtableString classes, as below:
//[NSString string]
//[[NSString alloc] init]
//[NSString new]
//[[NSMutableString alloc] init]
//[NSMutableString string]
//[NSMutableString new]
CxList UnRef = Find_UnknownReference();
CxList NSString = UnRef.FindByShortName("NSString");
NSString.Add(UnRef.FindByShortName("NSMutableString"));
CxList NSStringTarget = NSString.GetMembersOfTarget();
result.Add(NSStringTarget.FindByShortName("string"));
result.Add(NSStringTarget.FindByShortName("new"));
result.Add(NSStringTarget.GetMembersOfTarget().FindByShortName("init"));
result.Add(methods.FindByName("NSString"));
result.Add(methods.FindByName("NSNull"));
result.Add(NSStringTarget.FindByShortName("init"));