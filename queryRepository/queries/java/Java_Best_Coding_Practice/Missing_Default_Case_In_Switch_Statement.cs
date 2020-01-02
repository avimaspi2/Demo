CxList AllCase = Find_Cases();
CxList AllDefault = All.NewCxList();

foreach(CxList oneCase in AllCase)
{
	try
	{
		Case c = oneCase.TryGetCSharpGraph<Case>();
	
		if(c.IsDefault)
		{
			AllDefault.Add(c.NodeId, c);
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}

result = AllCase.GetAncOfType(typeof(SwitchStmt)) - AllDefault.GetAncOfType(typeof(SwitchStmt));