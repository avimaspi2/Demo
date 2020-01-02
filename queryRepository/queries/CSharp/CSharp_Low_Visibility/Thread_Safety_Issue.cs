if (All.isWebApplication)
{
	CxList logs = All.FindByName("*Log*", false) +
		All.FindByType("Logger");

	CxList no_logs = All - logs;

	CxList statics = no_logs.FindAllReferences(no_logs.FindByFieldAttributes(Modifiers.Static) - 
		no_logs.FindByFieldAttributes(Modifiers.Readonly));

	statics = statics - no_logs.FindByType(typeof(MethodInvokeExpr));

	CxList EventArgs = All.FindByType("*CommandEventArgs");
	
	CxList request = Find_Request();
	CxList request_classes = request.GetAncOfType(typeof(ClassDecl)).InheritsFrom("Page");

    CxList inputs = Find_Interactive_Inputs() - request.GetByAncs(request_classes);
	
	CxList methods = Find_Methods();
	
	// Ignore flows through the predicate of IEnumerable and ICollection filter methods:
	CxList filterMethodsFirst = methods.FindByShortNames(new List<string> {
			"All", "Any", "Count", "LongCount", "First", "FirstOrDefault","Last", "LastOrDefault",
			"Single", "SingleOrDefault", "TakeWhile","SkipWhile", "Where",
			"Find", "FindAll", "FindLastIndex", "FindIndex", "RemoveAll", "TrueForAll"});
	CxList filterMethodsSecond = methods.FindByShortNames(new List<string> { "Contains" });
	
	CxList filterMethodDefinitions = All.FindDefinition(filterMethodsFirst + filterMethodsSecond);
	CxList implementedFilterMethods = methods.FindAllReferences(filterMethodDefinitions);
	filterMethodsFirst -= implementedFilterMethods;
	filterMethodsSecond -= implementedFilterMethods;
	
	CxList lambda = All.FindByType(typeof(LambdaExpr));
	CxList returns = All.FindByType(typeof(ReturnStmt));
	CxList parameters = lambda.GetParameters(filterMethodsFirst, 0);
	parameters.Add(lambda.GetParameters(filterMethodsSecond, 1));
	CxList sanitizers = All.FindByFathers(returns.GetByAncs(parameters));
	
	result = (EventArgs + inputs).InfluencingOnAndNotSanitized(statics, sanitizers);
}