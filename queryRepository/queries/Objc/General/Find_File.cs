CxList strings = Find_Strings();

List<string> methodsNames = new List<string>() {@"*file:*", @"*file%@*"};
CxList fileMethods = strings.FindByShortNames(methodsNames);

result = fileMethods;