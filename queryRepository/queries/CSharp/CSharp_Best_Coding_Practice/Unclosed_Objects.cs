CxList close = Find_Methods().FindByName("*.close", false);
CxList closeTarget = close.GetTargetOfMembers();
CxList definitions = All.FindDefinition(closeTarget);
CxList methods = All.GetMethod(closeTarget);
foreach (CxList target in closeTarget)
{
	CxList method = methods.GetMethod(target);
	CxList targetDefinition = definitions.FindDefinition(target);
	CxList targetInForEach = targetDefinition.GetAncOfType(typeof(ForEachStmt));
	CxList methodDef = method.GetMethod(targetDefinition);
	if (targetInForEach.Count > 0 || methodDef.Count == 0)
	{
		close -= target.GetMembersOfTarget();
	}
}

CxList AllTrys = All.GetAncOfType(typeof(TryCatchFinallyStmt));
CxList fin = All.NewCxList();	

foreach(CxList oneTry in AllTrys)
{
	TryCatchFinallyStmt t = oneTry.data.GetByIndex(0) as TryCatchFinallyStmt;
	fin.Add(All.FindById(t.Finally.NodeId));
}
fin = All.GetByAncs(fin);

CxList Try = close.GetAncOfType(typeof(TryCatchFinallyStmt));
foreach(CxList oneTry in Try)
{
	TryCatchFinallyStmt TryGraph = oneTry.data.GetByIndex(0) as TryCatchFinallyStmt;
	CxList curTry = All.FindById(TryGraph.Try.NodeId);
	CxList TryClose = close.GetByAncs(curTry);
	CxList AllClose = close.GetByAncs(oneTry);
	
	if( (AllClose - TryClose).Count == 0)
	{
		if (TryClose.GetAncOfType(typeof(UsingStmt)).Count == 0)
		{
			result.Add(TryClose);
		}
	}	
}
 
result -= result.FindByFathers(fin);