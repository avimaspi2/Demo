CxList attributesWithExpr = Lightning_Find_All_Attributes_With_EL();
CxList relevantAttr = All.NewCxList(); 
foreach(CxList awe in attributesWithExpr)
{
	CxList actn = awe.FindByShortName("action");
	CxList element = cxXPath.GetElementByExpression(cxXPath.GetExpressionsByAttributes(actn));
	if(actn.Count > 0 && element.FindByShortName("handler", false).Count > 0)
	{
		relevantAttr.Add(awe);
	}
}
CxList eventObj = Lightning_Find_Controller_Event_Object();

Lightning_Map_Component_To_Controller();

Dictionary<string,string> cmpToControllerMapping = querySharedData.GetSharedData("Lightning_Cmp_To_Cont") as Dictionary<string,string>;
//getEvent
CxList getEvent = All.FindByShortName("getEvent");
CxList eventName = All.GetParameters(getEvent);
//Aget
CxList aGet = All.FindByShortName("$A").GetMembersOfTarget().FindByShortName("get");
CxList AEventName = All.FindByShortName("e.*").GetParameters(aGet);
//get
CxList getC = Lightning_Find_Controller_Component_Object().GetMembersOfTarget().FindByShortName("get");
CxList cEventName = All.FindByShortName("e.*").GetParameters(getC);
eventName.Add(AEventName);
eventName.Add(cEventName);
CxList setParam = Find_Methods().FindByShortName("setParam");
CxList prm = Find_Param();
CxList setParamTarget = setParam.GetTargetOfMembers();
CxList allButParam = All - prm;

CxList allGets = All.NewCxList();
allGets.Add(getC);
allGets.Add(aGet);
allGets.Add(getEvent);

foreach(CxList en in eventName)
{
	CxList evnt = allGets.FindByParameters(en).GetAssignee();
	CxList spEvent = setParamTarget.FindAllReferences(evnt);
	CxList sParam = spEvent.GetMembersOfTarget();
	CxList firstParam = allButParam.GetParameters(sParam, 0);
	foreach(CxList sprm in sParam)
	{
		CxList spFirst = firstParam.GetParameters(sprm, 0);
		CxList spSecond = allButParam.GetParameters(sprm, 1);
		CustomFlows.AddFlow(spSecond, spFirst);	
	}
	string name = en.GetName();
	
	if(name.IndexOf(":") > -1)
	{
		name = name.Remove(0, name.IndexOf(":") + 1);		
	}
	if(name.StartsWith("e."))
	{
		name = name.Remove(0, 2);
	}	
	
	foreach(CxList handlerAttr in relevantAttr)
	{
		CxList expression = cxXPath.GetExpressionsByAttributes(handlerAttr);		
		CxList element = cxXPath.GetElementByExpression(expression);
		foreach(CxList elem in element)
		{
			
			CxXmlNode node = elem.TryGetCSharpGraph<CxXmlNode>();				
		
			if(node != null)
			{
				string curEventName = node.GetAttributeValueByName("event");				
				if(curEventName.StartsWith("c:"))
				{
					curEventName = curEventName.Remove(0, 2);
				}
				if(!curEventName.ToLower().Equals(name.ToLower()))
				{				
					break;
				}
				
			}
			CSharpGraph exprGraph = expression.GetFirstGraph();
			if(cmpToControllerMapping.ContainsKey(exprGraph.LinePragma.FileName))
			{
				CxList funct = All.FindByShortName(expression).FindByFileName(cmpToControllerMapping[exprGraph.LinePragma.FileName]);
				CxList eventAccess = eventObj.GetByAncs(funct);
				CxList gp = eventAccess.GetMembersOfTarget().FindByShortName("getParam");
				CxList parampassed = All.GetParameters(gp);
				if(parampassed.FindByShortName(firstParam).Count > 0)
				{
					CxList sl = parampassed.FindByType(typeof(StringLiteral));
					CustomFlows.AddFlow(firstParam, sl);
					
				}
			}
			
		
		}
	}
}