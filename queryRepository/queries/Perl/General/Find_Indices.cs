// Retrn all indices
CxList indRefList = All.FindByType(typeof(IndexerRef));
foreach (CxList indRef in indRefList)
{
	try
	{
		IndexerRef ir = indRef.data.GetByIndex(0) as IndexerRef;
		ExpressionCollection indices = ir.Indices;
		if (indices != null)
		{
			foreach (Expression singleExpe in indices)
			{
				if (singleExpe != null)
				{
					result.Add(All.FindById(singleExpe.NodeId));
				}
			}
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}