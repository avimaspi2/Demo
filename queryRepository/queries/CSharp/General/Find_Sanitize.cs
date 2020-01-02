//Find_Sanitize
CxList methods = Find_Methods();
CxList HashSanitize = methods.FindByMemberAccess("MD5.ComputeHash");
HashSanitize.Add(methods.FindByMemberAccess("SHA1.ComputeHash"));
HashSanitize.Add(methods.FindByMemberAccess("HashAlgorithm.ComputeHash"));
HashSanitize.Add(methods.FindByMemberAccess("RIPEMD160.ComputeHash"));
HashSanitize.Add(methods.FindByMemberAccess("SHA256.ComputeHash"));
HashSanitize.Add(methods.FindByMemberAccess("SHA384.ComputeHash"));
HashSanitize.Add(methods.FindByMemberAccess("SHA512.ComputeHash"));

CxList repl = methods.FindByMemberAccess("String.Replace*");
repl.Add(methods.FindByMemberAccess("Regex.Replace*"));
repl.Add(methods.FindByMemberAccess("StringBuilder.Replace*"));
repl = All.GetParameters(repl, 0);

CxList guid = All.FindByType(typeof(ObjectCreateExpr)).FindByShortName("Guid", true);

result.Add(Find_Replace());
result.Add(Find_Parameters());
result.Add(Find_Integers());
result.Add(repl);
result.Add(HashSanitize);
result.Add(guid);

// Add methods that return information about the object - and not the object itself
CxList objectMethods = All.NewCxList();
objectMethods.Add(methods.FindByShortNames(new List<string> {"nameof", "typeof"}));
objectMethods.Add(All.FindByMemberAccess("*.GetType"));
objectMethods.Add(All.FindByMemberAccess("*.GetHashCode"));
objectMethods.Add(All.FindByMemberAccess("*.Equals"));

result.Add(objectMethods);