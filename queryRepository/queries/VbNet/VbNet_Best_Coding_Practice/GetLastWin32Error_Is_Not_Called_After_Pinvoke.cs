CxList importedMethods = All.FindByCustomAttribute("dllimport").GetAncOfType(typeof(MethodDecl));
CxList allImportedMethodsCalls = All.FindAllReferences(importedMethods) - importedMethods;
CxList allGLWECalls = All.FindAllReferences(All.FindByName("*GetLastWin32Error", false));
CxList allMethodsThatCallGLWE = allGLWECalls.GetAncOfType(typeof(MethodDecl));
CxList allMethodsThatCallImportedMethods = allImportedMethodsCalls.GetAncOfType(typeof(MethodDecl));

result = allImportedMethodsCalls.GetByAncs(allMethodsThatCallImportedMethods - allMethodsThatCallGLWE);