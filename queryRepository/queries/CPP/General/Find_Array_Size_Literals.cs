CxList arrays = Find_ArrayCreateExpr();
CxList num =  Find_Integer_Literals();
foreach(CxList ace in arrays)
{
	try{
		ArrayCreateExpr expression = ace.TryGetCSharpGraph<ArrayCreateExpr>();
		if(expression != null && expression.Sizes != null && expression.Sizes.Count > 0)
		{
			foreach(Expression e in expression.Sizes)
			{
				result.Add(num.GetByAncs(All.FindById(e.NodeId)));
			}
		}
	}catch(Exception e)
	{
		cxLog.WriteDebugMessage(e);
	}
}