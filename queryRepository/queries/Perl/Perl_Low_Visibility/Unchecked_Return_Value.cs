//	Unchecked return value
//  -----------------------
///////////////////////////////////////////////////////////////////////

// Relevant methods
CxList methodsWithReturnValue = Find_Methods_With_Return_Value();

// Remove methods using "or" (or "||")
CxList or = All.FindByShortName("||").FindByType(typeof(BinaryExpr));
methodsWithReturnValue -= methodsWithReturnValue.GetByAncs(or);

// Remove methods in conditions - the condition is a check already
CxList conditions = Find_Conditions();
methodsWithReturnValue -= methodsWithReturnValue.GetByAncs(conditions);

// Remove methods that are assigned to a variable
CxList assignMethods = methodsWithReturnValue.GetAncOfType(typeof(AssignExpr));
CxList _ = All.FindByShortName("$_").GetAncOfType(typeof(AssignExpr));
assignMethods -= _;
methodsWithReturnValue -= methodsWithReturnValue.GetByAncs(assignMethods);

// remove checks in ternary expressions
methodsWithReturnValue -= methodsWithReturnValue.GetByAncs(All.FindByType(typeof(TernaryExpr)));

// Also consider all the members assigned a value from these methods, and appear in conditions
CxList assignValues = All.GetByAncs(assignMethods).FindByAssignmentSide(CxList.AssignmentSide.Left);
assignValues -= assignValues.FindAllReferences(conditions.FindAllReferences(assignValues));


/// return the methods and the relevant assigned members
result = methodsWithReturnValue + assignValues;


// Ignore cases where autodie is used
CxList autodie = All.FindByType(typeof(Import)).FindByRegex("autodie"); // Find autodie
CxList noAutodie = All.FindByType(typeof(UnknownReference)).FindByShortName("autodie"); // find no autodie to exclude

CxList autoDieNamespaces = autodie.GetAncOfType(typeof(NamespaceDecl));   // results in this namespace will be removed
CxList keepResults = noAutodie.GetAncOfType(typeof(StatementCollection)); // but results in this stmt collection will be kept

// Remove autodie results
result -= result.GetByAncs(autoDieNamespaces) - result.GetByAncs(keepResults);