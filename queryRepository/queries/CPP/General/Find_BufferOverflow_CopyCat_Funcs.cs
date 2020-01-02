CxList methodInvokes = Find_Methods();

List<string> copyMethods = new List<string> {"strcpy", "lstrcpy", "wcscpy", "_tcscpy", "_mbscpy", "CopyMemory"};
List<string> catMethods = new List<string> {"strcat", "lstrcat"};

List<string> copyCatMethods = copyMethods;
copyCatMethods.AddRange(catMethods);

result = methodInvokes.FindByShortNames(copyCatMethods);