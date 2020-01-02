CxList aOutRed = Find_Win_Elem_Address();
aOutRed.Add(Find_Members("location.hash"));
aOutRed.Add(Find_Members("location.href"));
aOutRed.Add(Find_Members("navigate.href"));
aOutRed.Add(Find_Members("window.navigate"));
aOutRed.Add(Find_Members("document.URL"));
aOutRed.Add(Find_Members("document.URLUnencoded"));
aOutRed.Add(Find_Members("location.protocol"));

aOutRed.Add(Find_JQuery_Outputs_Redirection());
aOutRed.Add(Find_MsAjax_Outputs_Redirection());

// Exclude right parts of equals - a = window.location
CxList declarators = Find_Declarators();
declarators.Add(Find_FieldDecls());
CxList assign = Find_AssignExpr();
assign.Add(declarators);   // var a = b; -> a is a Declarator and is the father of b;


CxList right = Find_Assign_Rights();
// Exclude LambdaFunctions assigns
CxList rightLambdaAssign = right.FindByType(typeof(LambdaExpr)).GetAncOfType(typeof(AssignExpr));
CxList irrelevant = aOutRed.GetByAncs(assign - rightLambdaAssign);
irrelevant.Add(aOutRed.GetTargetsWithMembers(right, 10));
// Exclude Comparators (window.location != location)
CxList comparators = Find_Conditions();
irrelevant.Add(aOutRed.GetByAncs(comparators));

// Reconsider left of assignments - var a = f(location = window.location)
CxList leftSide = Find_Assign_Lefts();
// relevant location, window and other sinks are environment variables, declarators are for local variables.
leftSide -= declarators; 
CxList alsoRelevant = aOutRed.GetByAncs(leftSide);
alsoRelevant.Add(aOutRed.GetTargetsWithMembers(leftSide, 10));
// Remove comparators and right sides and restore left sides
aOutRed = aOutRed - irrelevant;

aOutRed.Add(alsoRelevant);

result = aOutRed;

CxList mofTargets = result.GetMembersOfTarget();
// Exclude the methods that do not affect the location
result.Add(mofTargets - mofTargets.FindByShortNames(new List <string> {
		"substring","substr","length","indexOf","lastIndexOf","slice","split"}));
result -= mofTargets.GetTargetOfMembers();

CxList variables = result * declarators;
variables.Add(result.FindByType(typeof(UnknownReference)));
result -= variables;

///  to remove from the results 	window.location.origin and location.origin.
CxList toRemove = All.NewCxList();
toRemove.Add(result.FindByMemberAccess("location.origin"));
result -= toRemove;
result.Add(React_Find_PropertyKeys().FindByShortName("href"));