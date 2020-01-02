result = All.FindByMemberAccess("Random.Next");
result.Add(All.FindByMemberAccess("Random.NextByte"));
result.Add(All.FindByMemberAccess("Random.NextDouble"));

/* Find calls to {Next,NextByte,NextDouble} inside a class definition that extends Random */
CxList relevantMethods = Find_Methods().FindByShortNames(new List<string> {"Next", "NextByte", "NextDouble"});
relevantMethods -= All.GetMembersOfTarget(); // Remove memberAccess

foreach(CxList method in relevantMethods)
{
	try
	{
		if (All.FindDefinition(method).Count != 0) // Ignore methods that are defined in the code
			continue;
		
		var methodClass = All.GetClass(method);
		ClassDecl classDecl = methodClass.GetFirstGraph() as ClassDecl;
		foreach (TypeRef baseType in classDecl.BaseTypes)
			if (baseType.ResolvedTypeName == "Random")
				result.Add(method);
	}
	catch (Exception e)
	{
		cxLog.WriteDebugMessage(e);
	}
}