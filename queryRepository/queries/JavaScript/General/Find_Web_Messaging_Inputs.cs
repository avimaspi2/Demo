//looks for web messaging inputs 
/*CxList allRefs = Find_Web_Message_Event().GetMembersOfTarget();
result = allRefs.FindByShortName("data");
result.Add(allRefs.FindByShortName("originalEvent").GetMembersOfTarget().FindByShortName("data"));*/

CxList data = All.FindByMemberAccess("*.data");
CxList potentialEvent = data.GetTargetOfMembers();
CxList ur = Find_UnknownReference();
CxList ev = ur.FindByShortNames(new List<string>{"e","*event","eve"}, false);
potentialEvent = ev * potentialEvent;
result = potentialEvent.GetMembersOfTarget();
CxList bind = Find_Methods().FindByShortName("bind");

List < string > eventTypeList = new List<string>(
	new string [] {"blur", "click", "dblclick", "focus", 
	"mousedown", "mouseenter", "mouseleave", "mousemove", 
	"mouseout", "mouseover", "mouseup", "resize", "scroll",
	"keydown", "keyup","keypress","submit","select","change"});

potentialEvent = data.GetTargetOfMembers();

CxList allString = Find_String_Literal();

CxList potentialEventType = Find_String_Short_Name(allString, eventTypeList, false);
foreach(CxList e in potentialEvent)
{
	CxList constRef = All.FindAllReferences(e.GetAncOfType(typeof(LambdaExpr)));
	CxList functionAsParam = constRef.GetTargetOfMembers();
                
	CxList curBind = bind.FindByParameters(All.FindAllReferences(functionAsParam));
                                
	CxList bindFirstParam = All.GetParameters(curBind, 0);  		
	CxList click = bindFirstParam * potentialEventType;     

	CxList potentialEvents = bindFirstParam.DataInfluencedBy(potentialEventType);
	click.Add(potentialEvents.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly));

	if(click.Count > 0)
	{
		CSharpGraph g = e.GetFirstGraph();
		result.data.Remove(g.NodeId);
		break;		
	}
}