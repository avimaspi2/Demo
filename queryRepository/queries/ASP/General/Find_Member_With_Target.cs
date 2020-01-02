// $ASP

// Usage:
// Find_Member_With_Target("ADODB.Stream", "Read")

// Sample code:
// Set obj = Server.CreateObject("ADODB.Stream")
// obj.Read

if (param.Length == 2)
{
	//string memberAccess = param[0] as string;
	string type = param[0] as string;
	string command = param[1] as string;
	//string serverobj = "server.createobject";
	//string obj = "createobject";
	command = command.ToLower();
	
	//CxList rightSide1 = All.FindByAssignmentSide(CxList.AssignmentSide.Right);
	//rightSide1 = rightSide1.FindByMemberAccess(serverobj) + rightSide1.FindByShortName(obj);

	//CxList relevantParam = All.FindByName(type, false);
	//CxList relevantParamFunc = All.FindByParameters(relevantParam);
	//CxList rightSide = rightSide1 * relevantParamFunc;
	
	//CxList leftSide = rightSide.GetAncOfType(typeof(AssignExpr));
	//leftSide = All.GetByAncs(leftSide).FindByAssignmentSide(CxList.AssignmentSide.Left);
	//leftSide = All.FindAllReferences(leftSide);
	
	CxList leftSide = All.FindAllReferences(Find_Object(type));
	
	CxList findCommand = leftSide.GetMembersOfTarget().FindByShortName(command, false);
	result = findCommand;
}