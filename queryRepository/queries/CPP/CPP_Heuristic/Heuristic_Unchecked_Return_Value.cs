//	Use of uninitialized pointer
//  ----------------------------
//  In this query we look for NULL-initialized pointers, that the user
//  tries to use although they have no actual value in this address.
///////////////////////////////////////////////////////////////////////

// Find undefined methods
CxList empty = Find_Empty_Methods();
CxList defs = All.FindDefinition(Find_Methods()) - empty;
CxList undefinedMethods = Find_Methods() - All.FindAllReferences(defs);
undefinedMethods -= undefinedMethods.FindByShortName("sizeof");
undefinedMethods -= undefinedMethods.FindByShortName("strlen");

// Find undefined methods in the right side of an assign expression
CxList rightSide = All.FindByAssignmentSide(CxList.AssignmentSide.Right);
CxList rightSideMethods = undefinedMethods * rightSide;

// Which variables we want to check
CxList testVars = All.FindByFathers(rightSideMethods.GetAncOfType(typeof(AssignExpr))).FindByAssignmentSide(CxList.AssignmentSide.Left)
	+ rightSideMethods.GetAncOfType(typeof(Declarator));

// If statements conditions - to sanitize the check
CxList conditions = Get_Conditions();

// If statements that contain the vaviables to check
CxList IfStmts = conditions.DataInfluencedBy(testVars).GetAncOfType(typeof(IfStmt));
CxList IfStmtsInside = All.GetByAncs(IfStmts);

// Assert is a sanitizer too
CxList assert = Find_Methods().FindByName("assert");
CxList assertParam = All.GetByAncs(assert);
assertParam -= assertParam.FindByShortName("assert");

// What we want to check for influence - the references of the variables
// and all parameters of functions
CxList toCheck = All.FindAllReferences(testVars) + All.GetParameters(All);

// Remove all atomic types
CxList builtinTypes = Find_Builtin_Types();
toCheck -= toCheck * builtinTypes;

CxList binary = Find_BinaryExpr();
CxList boolOrIntExpr =
	binary.FindByShortName("*") +
	binary.FindByShortName("/") +
	binary.FindByShortName("+") +
	binary.FindByShortName("-") + 
	binary.FindByShortName("<") +
	binary.FindByShortName(">") +
	binary.FindByShortName("==") +
	binary.FindByShortName("!=") +
	binary.FindByShortName("<>") +
	binary.FindByShortName("<=") +
	binary.FindByShortName(">=") +
	binary.FindByShortName("||") +
	binary.FindByShortName("&&") +
	Find_Unarys().FindByShortName("Not");

toCheck -= boolOrIntExpr;

// Find influence of relevant undefined methods on the things we want to check
result = toCheck.InfluencedByAndNotSanitized(undefinedMethods, IfStmtsInside + assertParam + All.FindByType(typeof (MethodRef)));
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);