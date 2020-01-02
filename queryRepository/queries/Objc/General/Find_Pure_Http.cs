CxList strings = Find_Strings();

List<string> methodsNames = new List<string>{@"*http:*",@"*http%@*"};

result = strings.FindByShortNames(methodsNames);