CxList binary = All.FindByType(typeof(BinaryExpr));
CxList booleanConditions = 
	binary.FindByShortName("<") +
	binary.FindByShortName(">") +
	binary.FindByShortName("==") +
	binary.FindByShortName("!=") +
	binary.FindByShortName("<>") +
	binary.FindByShortName("<=") +
	binary.FindByShortName(">=") +
	binary.FindByShortName("||") +
	binary.FindByShortName("&&") +
	All.FindByType(typeof(UnaryExpr)).FindByShortName("Not");

CxList nonSanitizer = All.FindByType(typeof(AssignExpr)).GetByAncs(booleanConditions);
booleanConditions -= nonSanitizer.GetAncOfType(typeof(BinaryExpr));

result = booleanConditions;