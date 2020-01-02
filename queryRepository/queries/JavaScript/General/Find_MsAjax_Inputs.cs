CxList methods = Find_Methods();
result = methods.FindByShortName("get_stateString");
result.Add(methods.FindByMemberAccess("WebRequest._resolveUrl"));