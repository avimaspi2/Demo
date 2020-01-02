CxList UsingStmts = All.FindByType(typeof(UsingStmt));
foreach (CxList UsingStmt in UsingStmts)
{
	UsingStmt UsingGraph = UsingStmt.TryGetCSharpGraph<UsingStmt>();
	if (UsingGraph.DeclaresResource)
	{
		foreach (Declarator d in UsingGraph.Declaration.Declarators)
		{
			result.Add(All.FindById(d.NodeId));	
		}
	}
}