CxList tables = Find_DB_Conn_DBO();

CxList occur = All.FindAllReferences(tables);
CxList allOccur = occur.GetMembersOfTarget();

allOccur.Add(All.FindAllReferences(
	All.InfluencedBy(occur).FindByAssignmentSide(CxList.AssignmentSide.Left)).GetMembersOfTarget());

List<string> methodsNames = new List<string> {
		"insert","update","delete*","drop*","execute*"};

result.Add(allOccur.FindByShortNames(methodsNames));