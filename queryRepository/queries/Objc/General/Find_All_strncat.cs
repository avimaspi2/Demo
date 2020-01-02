CxList methods = Find_Methods();
List<string> methodsNames = new List<string>{"strncat", "_strncat*", "_mbsncat*", "wcsncat*"};
result = methods.FindByShortNames(methodsNames);