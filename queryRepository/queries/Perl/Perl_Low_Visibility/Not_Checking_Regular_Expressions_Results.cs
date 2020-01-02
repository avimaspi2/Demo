CxList regex = Find_Regex().GetAncOfType(typeof(BinaryExpr));

// Remove methods in conditions - the condition is a check already
CxList conditions = All.GetByAncs(Find_Conditions());
regex -= regex.GetByAncs(conditions);

// Remove methods that are assigned to a variable
CxList assignMethods = regex.GetAncOfType(typeof(AssignExpr));
CxList conditionMethods = assignMethods.GetFathers();
CxList ancsDeclarators = regex.GetAncOfType(typeof(Declarator));
conditionMethods.Add(ancsDeclarators);
CxList _ = All.FindByShortName("$_").GetAncOfType(typeof(AssignExpr));
assignMethods -= _;
regex -= regex.GetByAncs(assignMethods);

// remove checks in ternary expressions
regex -= regex.GetByAncs(All.FindByType(typeof(TernaryExpr)));

// Also consider all the members assigned a value from these methods, and appear in conditions
CxList assignValues = All.GetByAncs(assignMethods).FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList conditionValues = (All.FindByAssignmentSide(CxList.AssignmentSide.Left)).GetByAncs(conditionMethods);

// return the methods and the relevant assigned members
result.Add(regex);
result.Add(assignValues);
result -= result.DataInfluencingOn(conditions.FindAllReferences(conditionValues));

// Remove results under  testing
result -= result.GetByAncs(All.FindByShortName("||"));