CxList AllCase = All.FindByType(typeof(Case));
CxList AllDefault = All.NewCxList();

foreach(CxList oneCase in AllCase)
{
	Case c = oneCase.data.GetByIndex(0) as Case;
	
	if(c.IsDefault)
	{
		AllDefault.Add(c.NodeId, c);
	}
}

result = AllCase.GetAncOfType(typeof(SwitchStmt)) - AllDefault.GetAncOfType(typeof(SwitchStmt));