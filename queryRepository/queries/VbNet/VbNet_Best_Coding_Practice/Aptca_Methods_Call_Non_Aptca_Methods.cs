CxList allMethods = All.FindByType(typeof(MethodDecl));
CxList aptcaMethods = All.FindByCustomAttribute("allowpartiallytrustedcallersattribute").GetAncOfType(typeof(MethodDecl));
CxList nonAptcaMethods = allMethods - aptcaMethods;
CxList allMethodsCalls = All.FindAllReferences(allMethods) - allMethods;
CxList allNonAptcaMethods = allMethods - aptcaMethods;
CxList allNonAptcaMethodsCalls = All.FindAllReferences(allNonAptcaMethods) - allNonAptcaMethods;
result = allNonAptcaMethodsCalls.GetByAncs(aptcaMethods);