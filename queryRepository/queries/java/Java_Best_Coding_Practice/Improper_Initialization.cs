CxList boolean = Find_BooleanLiteral();
CxList conditions = Find_Conditions();
CxList Not = All.FindByShortName("Not").FindByType(typeof(UnaryExpr));

// true:
// Find all true-initialized values inside negative conditions
// See if there's a true assignment inside the condition

CxList valueTrue = boolean.FindByShortName("true");
CxList assignedTrue = valueTrue.GetFathers();
CxList trueReferences = All.FindAllReferences(valueTrue.GetAncOfType(typeof(Declarator)));
CxList trueInConditions = trueReferences.GetByAncs(Not).GetByAncs(conditions);

CxList badInitialization = All.NewCxList();
foreach (CxList condition in trueInConditions){
	CxList referencesOfThis = trueReferences.FindAllReferences(condition) - condition;
	CxList VarsInCondition = referencesOfThis.GetByAncs(condition.GetAncOfType(typeof(IfStmt)));
	CxList assignToTrue = VarsInCondition.GetFathers().FindByType(typeof(AssignExpr)) * assignedTrue;
	CxList VarsAssignedToTrue = VarsInCondition.FindByFathers(assignToTrue);
	
	badInitialization.Add(VarsAssignedToTrue);
}

// false:
// Find all false-initialized values inside positive conditions
// See if there's a false assignment inside the condition

CxList valueFalse = boolean.FindByShortName("false");
CxList assignFalse = valueFalse.GetFathers();
CxList falseReferences = All.FindAllReferences(valueFalse.GetAncOfType(typeof(Declarator)));
CxList falseInConditions = falseReferences.GetByAncs(conditions);
falseInConditions -= falseInConditions.GetByAncs(Not);

foreach (CxList condition in falseInConditions){
	CxList referencesOfThis = falseReferences.FindAllReferences(condition) - condition;
	CxList VarsInCondition = referencesOfThis.GetByAncs(condition.GetAncOfType(typeof(IfStmt)));
	CxList assignToFasle = VarsInCondition.GetFathers().FindByType(typeof(AssignExpr)) * assignFalse;
	CxList VarsAssignedToFasle = VarsInCondition.FindByFathers(assignToFasle);
	
	badInitialization.Add(VarsAssignedToFasle);
}

result.Add(All.FindDefinition(badInitialization));