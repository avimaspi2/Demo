CxList methodsInvoke = Find_Methods();
//Regex(....)
result = Find_ObjectCreations().FindByName("Regex");
//Regex.fromLiteral(...) & .toRegex()
result.Add(methodsInvoke.FindByName("Regex.fromLiteral"));
result.Add(methodsInvoke.FindByShortName("toRegex"));