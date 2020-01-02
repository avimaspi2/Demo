CxList methods = Find_Methods();
CxList cicsMethods = methods.FindByShortName("EXEC_CICS");
CxList unknownRefs = Find_Unknown_References();
CxList relevantParameters = unknownRefs.FindByShortNames(new List<string>(){ "NOHANDLE", "IGNORE_CONDITION*"});

result = relevantParameters.GetByAncs(cicsMethods);