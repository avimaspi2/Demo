CxList methods = Find_Methods();
CxList potentialDomElements = Find_Potential_DOM_Elements();

CxList commands = Find_Members("document.open", methods);
commands.Add(Find_Members("window.open", methods));
commands.Add(Find_Members("window.navigate", methods));
commands.Add(Find_Members("location.replace"));
commands.Add(Find_Members("XMLHttpRequest.open")); 	//XML HTTP REQUEST

result = Find_Members("document.URL");
result.Add(Find_Members("document.URLUnencoded"));
result.Add(Find_Members("document.domain"));
result.Add(Find_Members("navigate.href"));
result.Add(Find_Members("location.search"));
result.Add(Find_Members("location.protocol"));
result.Add(Find_Win_Elem_Address());

CxList setter = Find_Set_Attribute_Structure();
CxList actions = All.FindByShortNames(new List<string>{"action","src"});
result.Add(setter * potentialDomElements.GetRightmostMember() * actions);
result.Add(setter.FindByParameters(actions.GetParameters(setter.FindByShortName("setAttribute"), 0)));

//consider only left of assignment, before adding parameters of open()
CxList assignmentSigns = result.GetAncOfType(typeof(AssignExpr));
CxList sonsOfAssign = All.GetByAncs(assignmentSigns);
CxList AllLeftSons = sonsOfAssign.FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList targetOfSon = AllLeftSons.GetTargetOfMembers();

for(int i = 0; i < 10; ++i)
{
	if (targetOfSon.Count <= 0)
	{
		break;
	}
	AllLeftSons.Add(targetOfSon);
	targetOfSon = targetOfSon.GetTargetOfMembers();
}
 
//Remove assignment to UnknownReferences
AllLeftSons -= AllLeftSons.FindByType(typeof(UnknownReference));

result = result * AllLeftSons;
 




//adding parameters of .open()
CxList commandParameters = All.GetParameters(commands);
CxList onlyParams = commandParameters.FindByType(typeof(Param));
CxList relevantParameters = commandParameters - onlyParams;
foreach(CxList cmd in onlyParams)
{
	CxList invoke = cmd.GetAncOfType(typeof(MethodInvokeExpr));
	result.Add(relevantParameters.GetByAncs(cmd)
		.ConcatenateAllTargets(invoke));
}

result.Add(Find_JQuery_Outputs_XSRF());

CxList mofTargets = result.GetMembersOfTarget();
result.Add(mofTargets - mofTargets.FindByShortNames(new List <string> {"substring","substr","length","indexOf","lastIndexOf","slice"}));
CxList toRemove = mofTargets.GetTargetOfMembers();
CxList variables = result.FindByType(typeof(Declarator));
toRemove.Add(variables);

//  to remove from the results 	window.location.origin and location.origin.
toRemove.Add(result.FindByMemberAccess("location.origin"));

CxList locHash = Find_Members("location.hash");
locHash.Add(locHash.GetTargetOfMembers());

toRemove.Add(locHash);

result -= toRemove;
result.Add(React_Find_PropertyKeys().FindByShortName("href"));