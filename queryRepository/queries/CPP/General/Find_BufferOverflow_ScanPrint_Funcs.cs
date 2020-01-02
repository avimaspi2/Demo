CxList methodInvokes = Find_Methods();

List<string> methodsScanPrint = new List<string>{"scanf", "vscanf", "fscanf", "vfscanf", "sscanf", "vsscanf", "sprintf"};

result = methodInvokes.FindByShortNames(methodsScanPrint);