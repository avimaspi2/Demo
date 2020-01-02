if (1 == param.Length && null != param[0] as CxList)
{	
	Dictionary<int, List<CxList>> Duplicates = new Dictionary<int, List<CxList>>();
	Func<CxList, int> HashElem = _ =>
		{
		CSharpGraph csg = _.GetFirstGraph();
		string N = csg.ShortName;
		int L = csg.LinePragma.Line;
		int C = csg.LinePragma.Column;
		GraphTypes T = csg.GraphType;
		return new Tuple<string,int,int,GraphTypes>(N, L, C, T).GetHashCode();	
		};
	
	Func<CxList, int> HashPath = _ =>
	{
		int hs = 0;
		foreach (CxList R in _.GetCxListByPath())
		{	
			hs = hs ^ HashElem(R);
		}
		return hs;
	};
	
	// Populate potential Duplicates dictionary
	foreach (CxList R in (param[0] as CxList).GetCxListByPath())
	{
		if (R.GetFirstGraph().LinePragma.FileName.EndsWith(".jsp"))
		{
			int H = HashPath(R);
			
			if (!Duplicates.ContainsKey(H))
			{
				Duplicates[H] = new List<CxList>();
			}
			Duplicates[H].Add(R);
		}
		else
		{
			result.Add(R);	
		}
	}
	
	// Traverse potential duplicates Duplicates dictionary
	foreach (List<CxList> L in Duplicates.Values)
	{
		if (L.Count > 1)
		{
			bool added = false;
			for (int i = 0; i + 1 < L.Count && !added; i++)
			{
				if (JSP_Does_Include(L[i], L[i + 1]).Count > 0)					
				{
					result.Add(L[i + 1]);
					added = true;
					
				}
				else if (JSP_Does_Include(L[i + 1], L[i]).Count > 0)
				{
					result.Add(L[i]);
					added = true;
				}
				else
				{
					// Then both L[i] and L[i+1] are including a third file.
					// Keep traversing so we can find it.
				}
			}
			
			if (!added)
			{
				foreach (CxList R in L)
				{
					result.Add(R);
				}
			}
		}
		else
		{
			result.Add(L[0]);	
		}
	}
}