CxList methods = Find_Methods();
CxList allParams = All.FindByType(typeof (Param));
result = All.FindByType(typeof(StringLiteral)) +
	allParams.GetParameters(methods.FindByShortName("q"));