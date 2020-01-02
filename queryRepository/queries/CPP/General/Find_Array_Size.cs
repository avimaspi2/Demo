// given an array reference (of type CSharpGraph), this query tries to return the size the array
// returns empty CxList if can't find the size


if(param.Length == 1 ){
	CSharpGraph arrayRef = ((CxList)param[0]).TryGetCSharpGraph<CSharpGraph>();
	CxList arrayUse = All.NewCxList();
	arrayUse.Add(arrayRef.NodeId, arrayRef);
	
	CxList arrayDecl = All.FindDefinition(arrayUse);
	
	CxList all_integers = Find_Integer_Literals();
	CxList declLiteralInt = all_integers.GetByAncs(arrayDecl);
	
	if(declLiteralInt.Count > 0)
		result = declLiteralInt;
	else{
		List < string > names = new List<string>(){"malloc","calloc","aligned_alloc","realloc"};
		CxList all_allocs = Find_Methods().FindByShortNames(names);
		
		CxList alloc_assigns = all_allocs.GetAncOfType(typeof(AssignExpr));
		CxList array_allocs = All.FindAllReferences(arrayUse).GetByAncs(alloc_assigns).GetAssigner();
		
		if(array_allocs.Count > 0){
			if(array_allocs.Count == 1){
				result = all_integers.GetByAncs(array_allocs);
			}
			else{
				CSharpGraph last = (CSharpGraph) array_allocs.data.GetByIndex(array_allocs.data.Count - 1);
				CxList ret = All.FindById(last.NodeId);
				result = all_integers.GetByAncs(ret.GetAssigner());
			}
		}
	}
}