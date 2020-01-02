//	Find All varibles in indexes
CxList indxRef = Find_IndexerRefs();
CxList possibleIndexes = Find_Reference().FindByFathers(indxRef);
possibleIndexes -= possibleIndexes.FindByType("string");
possibleIndexes = possibleIndexes.FindByType(typeof(UnknownReference));
CxList indx = All.NewCxList();
foreach (CxList ind in indxRef)
{
	IndexerRef p = ind.data.GetByIndex(0) as IndexerRef;
	foreach (Expression ex in p.Indices)
	{
		try
		{
			indx.Add(possibleIndexes.FindById(ex.NodeId));
		}
		catch(Exception)
		{
			cxLog.WriteDebugMessage("Error while accessing index");
		}
	}
}

// Remove all indexes "sanitized" by a condition
CxList variblesInCondintions = Get_Conditions();
CxList indexReferencesInConditions = variblesInCondintions.FindAllReferences(indx);
CxList goodIndexes = All.NewCxList();
foreach (CxList checkedReference in indexReferencesInConditions)
{
	CxList condition = checkedReference.GetAncOfType(typeof(IfStmt));
	condition.Add(checkedReference.GetAncOfType(typeof(IterationStmt)));
	CxList checkedIndexes = indx.FindAllReferences(checkedReference).GetByAncs(condition);
	goodIndexes.Add(checkedIndexes);
}
CxList problems = indx - goodIndexes;

result = problems.GetByAncs(problems.GetAncOfType(typeof(IndexerRef)).FindByAssignmentSide(CxList.AssignmentSide.Left));