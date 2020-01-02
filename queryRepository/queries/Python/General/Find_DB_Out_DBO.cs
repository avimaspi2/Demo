CxList tables = Find_DB_Conn_DBO();
CxList occur = All.FindAllReferences((tables));
CxList allOccur = occur.GetMembersOfTarget();

allOccur.Add(All.FindAllReferences(
	All.InfluencedBy(occur).FindByAssignmentSide(CxList.AssignmentSide.Left)).GetMembersOfTarget());

result.Add(allOccur.FindByShortName("select*"));