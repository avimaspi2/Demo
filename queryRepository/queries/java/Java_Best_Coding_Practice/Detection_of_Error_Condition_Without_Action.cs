CxList Catch = Find_Catch();
foreach(CxList curCatch in Catch)
{
	Catch ch = curCatch.TryGetCSharpGraph<Catch>();
	if(ch.Statements.Count == 0)
	{
		result.Add(ch.NodeId, ch);
	}
}

CxList mkdirs = All.FindByMemberAccess("File.mkdirs");
CxList not = mkdirs.GetAncOfType(typeof(UnaryExpr)).FindByShortName("Not");
CxList If = not.GetFathers().FindByType(typeof(IfStmt));
foreach(CxList curIf in If)
{
	IfStmt ifStmt = curIf.TryGetCSharpGraph<IfStmt>();
	if(ifStmt.TrueStatements.Count == 0)
	{
		result.Add(ifStmt.NodeId, ifStmt);
	}
}