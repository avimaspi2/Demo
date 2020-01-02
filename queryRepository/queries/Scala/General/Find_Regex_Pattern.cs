CxList methods = Find_Methods();
result = methods.FindByShortNames(new List<string>{"find", "lookingAt"});
result.Add(methods.FindByMemberAccess("Pattern.compile"));