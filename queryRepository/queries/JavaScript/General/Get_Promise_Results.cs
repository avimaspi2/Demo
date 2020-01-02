if(param.Length <= 1){
	CxList elems = param[0] as CxList;
	
	CxList right = elems.FindByAssignmentSide(CxList.AssignmentSide.Right);
	CxList chained = elems - right;
	
	CxList left = right.GetAssignee();
	CxList refs = All.FindAllReferences(left);
	
	result.Add(All.GetParameters(refs.GetMembersOfTarget()));
	result.Add(chained.GetMembersOfTarget());
	
	result.Add(All.FindByParameters(chained).FindByShortName("Array").GetAssignee() );
}