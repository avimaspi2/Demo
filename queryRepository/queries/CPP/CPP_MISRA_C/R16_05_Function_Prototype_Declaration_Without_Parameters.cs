/*
MISRA C RULE 16-5
------------------------------
This query searches for prototypes (function declarations) with no parameters (should have void parameter)

	The Example below shows code which does not conform to the MISRA rule: 

void foo ();

If 'foo' is a function without parameter, its prototype should be written this way:

void foo (void);

*/

// Find all function declarations
CxList methodDeclsAndDefs = All.FindByType(typeof(MethodDecl));
CxList methodDefs = All.FindByType(typeof(StatementCollection)).GetFathers().FindByType(typeof(MethodDecl));
CxList methodDecls = methodDeclsAndDefs - methodDefs;

// Find those without parameter
CxList parameters = All.GetParameters(methodDecls);
CxList methodDeclsWithParams = parameters.GetAncOfType(typeof(MethodDecl));
CxList methodDeclsWithoutParams = methodDecls - methodDeclsWithParams;

/*
  Parsing removes 'void' parameters, hence we need to drop relevant functions here using FindByRegex
*/

// Find those with 'void' parameter
CxList methodDeclsWithVoidParam = methodDeclsWithoutParams.FindByRegex(@"\s*\(\s*void\s*\)");

result = methodDeclsWithoutParams - methodDeclsWithVoidParam;