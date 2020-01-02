if (param.Length == 1)
{
	CxList reference = param[0] as CxList;

	CxList allBinaryExprs = reference.FindByType(typeof(BinaryExpr));
	CxList leftBinary = allBinaryExprs;

	foreach (CxList curBe in allBinaryExprs)
	{
		BinaryExpr be = curBe.data.GetByIndex(0) as BinaryExpr;
		if (be != null && be.Left is BinaryExpr) //this is not leftmost binary
		{
			leftBinary -= curBe;
		}
	}

	CxList leftBinaryValues = All.NewCxList();
	foreach (CxList curB in leftBinary)
	{
		BinaryExpr be = curB.data.GetByIndex(0) as BinaryExpr;
		if (be.Left != null)//add all children of leftmost binary
		{
			leftBinaryValues.Add(reference.GetByAncs(All.FindById(be.Left.NodeId)));
		}
	}

	//return all binary which are not leftmost childrens
	result = reference.GetByAncs(allBinaryExprs) - allBinaryExprs - leftBinaryValues;
}