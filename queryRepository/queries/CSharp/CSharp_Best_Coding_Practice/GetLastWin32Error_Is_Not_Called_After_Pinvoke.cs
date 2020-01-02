CxList importedMethods = All.FindByCustomAttribute("DllImport").GetAncOfType(typeof(MethodDecl));
CxList allImportedMethodsCalls = All.FindAllReferences(importedMethods) - importedMethods;
CxList allGLWECalls = All.FindAllReferences(All.FindByName("*GetLastWin32Error"));
CxList allMethodsThatCallGLWE = allGLWECalls.GetAncOfType(typeof(MethodDecl));
CxList allMethodsThatCallImportedMethods = allImportedMethodsCalls.GetAncOfType(typeof(MethodDecl));

result = allImportedMethodsCalls.GetByAncs(allMethodsThatCallImportedMethods - allMethodsThatCallGLWE);
result = result.FindByType(typeof(MethodInvokeExpr));