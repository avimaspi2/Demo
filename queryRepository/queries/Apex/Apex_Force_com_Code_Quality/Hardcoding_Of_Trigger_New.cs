CxList triggers = Find_Triggers_Code();
CxList triggerNew = triggers.FindByMemberAccess("trigger.New", false);
CxList triggerIndexerRef = triggerNew.GetFathers().FindByType(typeof(IndexerRef));

// Find all indexes of Trigger.New
CxList triggersIndexes = All.NewCxList();
foreach (CxList tr in triggerIndexerRef)
{
	IndexerRef ir = tr.data.GetByIndex(0) as IndexerRef;
	if (ir != null)
	{
		foreach (Expression ex in ir.Indices)
		{
			triggersIndexes.Add(All.FindById(ex.NodeId).FindByShortName("0"));
		}		
	}
}

result = triggerNew.GetByAncs(triggersIndexes.GetFathers());

result -= Find_Test_Code();