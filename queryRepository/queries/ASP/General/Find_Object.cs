// $ASP

// Usage:
// Find_Object("ADODB.Stream")

// Sample code:
// Set obj = Server.CreateObject("ADODB.Stream")


if (param.Length == 1)
{
	string type = param[0] as string;
	if (type != null){
		if (!type.EndsWith("*")){
			type = type + "\""; //string literals are wrapped with double quotes
		}
		if (!type.StartsWith("*")){
			type = "\"" + type; //string literals are wrapped with double quotes
		}
		string serverobj = "server.createobject";
		string obj = "createobject";
	
		CxList rightSide1 = All.FindByAssignmentSide(CxList.AssignmentSide.Right);
		rightSide1 = rightSide1.FindByMemberAccess(serverobj) + rightSide1.FindByShortName(obj);

		CxList relevantParam = All.FindByName(type, false);
		CxList relevantParamFunc = All.FindByParameters(relevantParam);
		CxList rightSide = rightSide1 * relevantParamFunc;
	
		CxList leftSide = rightSide.GetAncOfType(typeof(AssignExpr));
		leftSide = All.GetByAncs(leftSide).FindByAssignmentSide(CxList.AssignmentSide.Left);
		//leftSide = All.FindAllReferences(leftSide);

		result = leftSide;
	}
}