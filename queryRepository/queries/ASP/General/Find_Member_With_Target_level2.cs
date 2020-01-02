// $ASP

// Usage:
// Find_Member_With_Target_level2("Scripting.FileSystemObject", "m1", "m2")

// Sample code:
// Set obj1 = Server.CreateObject("Scripting.FileSystemObject") 
// Set obj2 = obj1.m1
// obj2.m2

if (param.Length == 3)
{
	string type = param[0] as string;
	string command = param[1] as string;
	string m2param = param[2] as string;
	
	CxList m1 = Find_Member_With_Target(type, command);	
	
	CxList rightSide1 = All.FindByAssignmentSide(CxList.AssignmentSide.Right);
	CxList rightSide = All.GetByAncs(rightSide1) * m1;
	
	CxList leftSide = rightSide.GetAncOfType(typeof(AssignExpr));
	leftSide = All.GetByAncs(leftSide).FindByAssignmentSide(CxList.AssignmentSide.Left);
	leftSide = All.FindAllReferences(leftSide);
	
	CxList findCommand = leftSide.GetMembersOfTarget().FindByShortName(m2param, false);
	result = findCommand;
}