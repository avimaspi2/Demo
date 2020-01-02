CxList methods = Find_Methods();
List<string> methodsNames = new List<string>{"snprintf", "_snprintf*", "_snwprintf*", "vsnprintf"};
result = methods.FindByShortNames(methodsNames);