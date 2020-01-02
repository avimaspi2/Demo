/* Consider a method declaration M and a flow path starting at one of
   M's parameter declaration, say h, like so:

   (0)    function M (..., h, ...) { ... }
   
   where there is some flow path starting at h, say h->a_1->....->a_n.

   Consider further that there are (in the source-code) method invocations
   where the symbol M occurs as a parameter, like so:

   (1)    someObject.someMethod(..., M, ...)

   This query's purpose is then to return the flow path starting at M
   in (1), followed by M in (0), followed by h->a_1->...->a_n.

                          * * *

   param[0]            : a set of h->a_1->...->a_n flows.
   param[1] (Optional) : a set of occurrences of M as a parameter to other method.
*/
if (param.Length > 0 && null != param[0] as CxList)
{	
	CxList hPaths = param[0] as CxList;
	CxList unknownRefs = Find_UnknownReference();
	CxList methodDecls = Find_MethodDecls();
	
	CxList hs = hPaths.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);	
	CxList MsMethodDecls = methodDecls.CallingMethodOfAny(hs);
	CxList MsParams = (param.Length == 2 && null != param[1] as CxList) ? param[1] as CxList : unknownRefs.FindAllReferences(MsMethodDecls);
	
	CxList prefixPaths = All.NewCxList();
	foreach (CxList m in MsParams)
	{
		CxList mMethodDecl = MsMethodDecls.FindDefinition(m);
		CxList mH = hs.GetParameters(mMethodDecl);
		prefixPaths.Add(m.ConcatenatePath(mMethodDecl).ConcatenatePath(mH));
	}
	
	result.Add(Get_Composed_Path(prefixPaths, hPaths));
}