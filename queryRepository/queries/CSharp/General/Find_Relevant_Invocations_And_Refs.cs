CxList methods = Find_Methods();
methods = methods.FindAllReferences(Find_MethodDecls().FindDefinition(methods));
CxList reference = Find_Unknown_References();
CxList relevantOnly = reference.GetAncOfType(typeof(ObjectCreateExpr)) + reference.GetAncOfType(typeof(MethodInvokeExpr));
reference = reference.GetByAncs(relevantOnly);


result.Add(methods);
result.Add(reference);