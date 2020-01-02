// All getters/setters
CxList methodDecl = Find_MethodDeclaration();
CxList getters = methodDecl.FindByShortName("get*");
getters.Add(methodDecl.FindByShortName("is*"));

CxList assignLeftSide = Find_AssignLeftSide();

// Find the definition of the assign left
CxList assignedLeft = All.FindDefinition(assignLeftSide);
// Find all references of the assign left, except for their definition
CxList assignLeftSideRef = All.FindAllReferences(assignLeftSide) - assignedLeft;

// Remove places where the variable is called in a getter
CxList gettersRef = All.FindAllReferences(assignedLeft).GetByAncs(getters);
CxList gettersReturnRef = All.GetByAncs(Find_ReturnStmt()).FindAllReferences(gettersRef);
gettersRef = gettersRef.FindAllReferences(gettersReturnRef);
assignedLeft -= assignedLeft.FindAllReferences(gettersReturnRef);
CxList staticAssignedLeft = (assignedLeft.GetAncOfType(typeof(FieldDecl)) * Find_Field_Decl()).FindByFieldAttributes(Modifiers.Static);
assignedLeft -= assignedLeft.GetByAncs(staticAssignedLeft);

// Remove variables in an if-statement, because they might be not set if the if-condition is false
// (using the most exact possible notation (using GetFathers instead of GetByAnc)
CxList fathers3 = assignLeftSide.GetFathers().GetFathers().GetFathers();

CxList allFathers3 = fathers3.GetFathers();
allFathers3.Add(fathers3);

CxList assignLeftSideNotIf = allFathers3.FindByType(typeof(IfStmt));
assignLeftSide -= assignLeftSide.GetByAncs(assignLeftSideNotIf);

// Sanitize all data out of the relevant class, so that the InfluencingOn in the following loop is faster
CxList allAssignLeftSide = All.NewCxList();
allAssignLeftSide.Add(assignLeftSideRef);
allAssignLeftSide.Add(assignLeftSide);

CxList assignLeftClasses = All.GetClass(allAssignLeftSide);
CxList sanitize = All - All.GetByAncs(assignLeftClasses);
sanitize.Add(Find_Dead_Code());

// Find all cases where there's an assign that is not influencing anything
CxList assignLeftRef = assignLeftSide.FindAllReferences(assignedLeft);
assignLeftRef -= assignedLeft;
assignLeftRef -= sanitize;

result = assignLeftRef;