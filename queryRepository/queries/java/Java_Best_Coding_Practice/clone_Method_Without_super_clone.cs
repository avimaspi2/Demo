CxList cloneable = All.InheritsFrom("Cloneable");
CxList methodDecl = Find_MethodDeclaration();
CxList methods = Find_Methods();

CxList clone = methodDecl.GetByAncs(cloneable).FindByShortName("clone");

// super.clone call. Using FindByShortName, "super" is not a standard member.
// It sould catch 99.9 % of te cases
CxList superClone = methods.FindByShortName("clone");
// Find the methos that contains the super.finalize
CxList cloneWithSuperFinalize = superClone.GetAncOfType(typeof(MethodDecl));

//The result is all the finalize method not containing super.clone.
result = clone - cloneWithSuperFinalize;