/* Given two sets of paths, say X and Y, return the set 
   resulting from composing each path in Y with each path in X.

   We define the composition of paths as follows. 
   Let A = a_1, a_2, ..., a_n be a path starting at node a_1 and ending at node a_n.
   Let B = b_1, b_2, ..., b_m be another path with a similar structure.
   The composition of B with A is defined as the path
   (0) a_1, a_2, ..., a_n, b_2, ..., b_m          , if a_n == b_1
   (1) undefined                                  , otherwise. 
*/
if (param.Length == 2 && null != param[0] as CxList && null != param[1] as CxList)
{
	CxList A = param[0] as CxList;
	CxList B = param[1] as CxList;
		
	foreach (CxList p_b in B.GetCxListByPath())
	{
		CxList b_1 = p_b.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
		foreach (CxList p_a in A.GetCxListByPath())
		{
			CxList a_n = p_a.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
			if (a_n == b_1)
				result.Add(p_a.ConcatenatePath(p_b));
		}				
	}
}