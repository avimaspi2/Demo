/* heuristic find the mongoose package db in methods */
CxList mongoose = Find_Require("mongoose");

if(mongoose.Count > 0)
{
	CxList modelMA = All.FindByMemberAccess("*.model");
	CxList leftSide = All.FindAllReferences(Find_Assign_Lefts().GetByAncs(modelMA.GetFathers()));
	
	CxList unkr = Find_UnknownReference();
	CxList models = All.NewCxList();
	foreach(CxList leftAssign in leftSide)
	{
		try
		{
			CSharpGraph g = leftAssign.GetFirstGraph() as CSharpGraph;
			if(g != null && g.LinePragma != null && g.ShortName != null)
			{
				models.Add(unkr.FindByFileId(g.LinePragma.GetFileId()).FindByShortName(g.ShortName));
			}
		}
		catch(Exception e)
		{
			cxLog.WriteDebugMessage(e);
		}
	}
		
	result = modelMA;
	result.Add(models);
}