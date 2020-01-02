if (param.Length == 2)
{
	string memberAccess = param[0] as string;
	string type = param[0] as string;
	string command = param[1] as string;
	string serverobj = "server.createobject";
	
	CxList rightSide1 = All.FindByAssignmentSide(CxList.AssignmentSide.Right);
	rightSide1 = rightSide1.FindByMemberAccess(serverobj);
	
	type = "\"" + type + "\"";
	CxList relevantParam = All.FindByName(type, false);
	CxList relevantParamFunc = All.FindByParameters(relevantParam);
	CxList rightSide = rightSide1 * relevantParamFunc;

	
	CxList leftSide = rightSide.GetAncOfType(typeof(AssignExpr));
	leftSide = All.GetByAncs(leftSide).FindByAssignmentSide(CxList.AssignmentSide.Left);
	leftSide = All.FindAllReferences(leftSide);
	
	CxList findCommand = leftSide.GetMembersOfTarget().FindByShortName(command,false);
	result = findCommand;
}