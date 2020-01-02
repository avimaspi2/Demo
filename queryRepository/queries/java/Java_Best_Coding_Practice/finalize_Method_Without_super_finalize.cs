// Find all method decl and method def
CxList methodDecl = Find_MethodDeclaration();
CxList methods = Find_Methods();

// finalize methods
CxList finalize = methodDecl.FindByShortName("finalize");

// super.finalize call. Using FindByShortName, "super" is not a standard member.
// It sould catch 99.9 % of te cases
CxList superFinalize = methods.FindByShortName("finalize");
// Find the methos that contains the super.finalize
CxList finalizeWithSuperFinalize = superFinalize.GetAncOfType(typeof(MethodDecl));

//The result is all the finalize method not containing super.finalize.
result = finalize - finalizeWithSuperFinalize;