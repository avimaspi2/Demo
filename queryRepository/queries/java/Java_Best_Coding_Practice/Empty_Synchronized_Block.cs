CxList synchronized = Find_LockStmt();

foreach (CxList s in synchronized)
{
	try
	{
		LockStmt syncStmt = s.TryGetCSharpGraph<LockStmt>();
		CxList syncStatements = All.FindById(syncStmt.Statements.NodeId);
		if (All.GetByAncs(syncStatements).Count == 1)
		{
			result.Add(syncStmt.NodeId, syncStmt);
		}
	}
	catch (Exception ex)
	{

	}
}