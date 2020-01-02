CxList reactJSAll = querySharedData.GetSharedData("ReactJSAll") as CxList;
if(reactJSAll != null && reactJSAll.Count > 0)
{
	CxList thisRefMembers = reactJSAll.FindByType(typeof(ThisRef)).GetMembersOfTarget();
	CxList methods = Find_Methods();
	CxList methodDecls = Find_MethodDecls();
	CxList unknownReferences = Find_UnknownReference();
	CxList leftSideUnknownReferences = unknownReferences.FindByAssignmentSide(CxList.AssignmentSide.Left);
	CxList react = unknownReferences.FindByShortName("React");
	react.Add(Find_Require("react"));
	CxList reactFirstMember = react.GetMembersOfTarget();
	CxList reactClasses = reactFirstMember.FindByShortName("createClass");
	reactClasses = Get_Class_Of_Anonymous_Ref(unknownReferences.GetByAncs(reactClasses));

	CxList setState = thisRefMembers.FindByShortNames(new List<string> {"setState", "replaceState"}) * methods;
	CxList getInitialState = methodDecls.FindByShortName("getInitialState");
	CxList state = thisRefMembers.FindByShortName("state");
	CxList anonyUnknownRefrences = unknownReferences.FindByShortName("anony*");

	foreach(CxList reactClass in reactClasses)
	{
		//Caching the result of GetByAncs because reactClass is a single node.
		CxList allUnderReactClass = reactJSAll.GetByAncs(reactClass);
		//GetInitialState
		CxList relevantGetInitalState = getInitialState * allUnderReactClass;
		CxList unknownReferencesUnderGetInitialState = anonyUnknownRefrences.GetByAncs(relevantGetInitalState);
		CxList anonyClassOfGetInitialState = Get_Class_Of_Anonymous_Ref(unknownReferencesUnderGetInitialState);
		CxList keysOfState = leftSideUnknownReferences.GetByAncs(anonyClassOfGetInitialState);
		//SetState
		CxList setStateInvokes = setState * allUnderReactClass;
		CxList setStateParameter = anonyUnknownRefrences.GetParameters(setStateInvokes, 0);
		CxList setStateAnonyObjectClass = Get_Class_Of_Anonymous_Ref(setStateParameter);
		keysOfState.Add(leftSideUnknownReferences.GetByAncs(setStateAnonyObjectClass));

		CxList relevantStateAccess = state * allUnderReactClass;
		CxList memberOfState = relevantStateAccess.GetMembersOfTarget();
		foreach(CxList key in keysOfState) 
		{
			string keyName = key.GetName();
			if(string.IsNullOrEmpty(keyName))
			{
				continue;
			}
			CxList relevantSinks = memberOfState.FindByShortName(keyName);
			CxList sinksWithMembers = relevantSinks.GetTargetsWithMembers();
			CxList sinksWithoutMembers = relevantSinks - sinksWithMembers;
			sinksWithoutMembers.Add(sinksWithMembers.GetRightmostMember());
			CustomFlows.AddFlow(key, sinksWithoutMembers);
		}
	}
}