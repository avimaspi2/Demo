// $VB6

// Usage:
// Find_Member_With_Target("ADODB.Stream", "Read")

// Sample code:
// Set obj = = CreateObject("ADODB.Stream")
// obj.Read

if (param.Length == 2)
{
	string type = param[0] as string;
	string command = param[1] as string;

	command = command.ToLower();
	CxList leftSide = All.FindAllReferences(Find_Object(type));	
	CxList findCommand = leftSide.GetMembersOfTarget().FindByShortName(command, false);
	result = findCommand;
}