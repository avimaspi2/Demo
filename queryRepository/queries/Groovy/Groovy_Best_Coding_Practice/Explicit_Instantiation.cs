// Find explicit instantiations that's can be replaced in pure Groovy simple syntax
CxList newObject = Find_Object_Create();
CxList list = All.NewCxList();

foreach(CxList item in newObject)
{
	foreach (KeyValuePair<int, IGraph> dic in item.data)
	{
		ObjectCreateExpr newExpr = dic.Value as ObjectCreateExpr;
		if ((newExpr.FullName.Equals("LinkedHashMap") && newExpr.Parameters.Count == 0)
			|| (newExpr.FullName.Equals("HashSet") && newExpr.Parameters.Count == 0)
			|| (newExpr.FullName.Equals("LinkedList") && newExpr.Parameters.Count == 0)
			|| (newExpr.FullName.Equals("Stack") && newExpr.Parameters.Count == 0)
			|| (newExpr.FullName.Equals("TreeSet") && newExpr.Parameters.Count == 0))
		{
			list.Add(dic);
		}
	}
}

result = list;