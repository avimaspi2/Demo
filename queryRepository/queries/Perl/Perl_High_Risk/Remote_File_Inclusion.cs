CxList methods = Find_Methods();
CxList include = methods.FindByShortName("require", false) + 
	methods.FindByShortName("use", false); // TODO: fix when uses enter DOM
include = All.FindByType(typeof(Import)) + All.FindByType(typeof(ExprStmt)).FindByRegex(@"require\s+\w");

result = include.DataInfluencedBy(Find_Interactive_Inputs());