// VB6

// Usage:
// Find_Member_With_Target("Connection", "Execute")

// Sample code:
// Set obj = New ADODB.Connection
// obj.Execute

if (param.Length == 2)
{
	
	string type = param[0] as string;
	string command = param[1] as string;
	
	if (type != null && command != null)
	{
		type = type.ToLower();
		CxList createType = All.FindByType(type);
		
		CxList createExp = All.FindByType(typeof(ObjectCreateExpr));
		CxList typeCreateExp = createExp.FindByShortName(type, false) + 
			createExp.GetMembersOfTarget().FindByShortName(type, false);
		
		CxList leftSideObject = typeCreateExp.GetAncOfType(typeof(AssignExpr));
		leftSideObject = All.GetByAncs(leftSideObject).FindByAssignmentSide(CxList.AssignmentSide.Left);
		
		CxList TypeObjects = All.GetByAncs(createType) + leftSideObject;
		CxList TypeRef = All.FindAllReferences(TypeObjects).FindByType(typeof(UnknownReference));
		result = TypeRef.GetMembersOfTarget().FindByShortName(command, false);
		
	}
}