CxList methods = All.FindByType(typeof(MethodInvokeExpr));

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
	All.FindByType(typeof(UnaryExpr)).FindByShortName("!");

CxList repl = methods.FindByShortName("replace", false);
repl = All.GetParameters(repl, 1);

result = methods.FindByShortName("escape") +
	methods.FindByShortName("*encode*") - methods.FindByShortName("*unencode*") +
	methods.FindByShortName("count") +
	methods.FindByShortName("indexof") +
	methods.FindByShortName("parseint") +
	methods.FindByShortName("parsefloat") +
	methods.FindByShortName("number") +
	methods.FindByShortName("length") +
	methods.FindByShortName("len") +
	booleanConditions;