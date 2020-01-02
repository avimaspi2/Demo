//this query will map ccontroller setter to the access inside the component
Lightning_Map_Component_To_Controller();
CxList allCmpRefs = Lightning_Find_Controller_Component_Object();
CxList setters = allCmpRefs.GetMembersOfTarget().FindByShortName("set");
CxList allInController = Lightning_Find_All_In_Controller();
CxList parameters = allInController.FindByType(typeof(Param));
CxList allSetterParameters = (All - parameters).GetParameters(setters);
CxList zeroParams = All.NewCxList();
	//create flow within a setter
foreach(CxList setter in setters)
{
	CxList zeroParam = allSetterParameters.GetParameters(setter, 0);
	CxList oneParam = allSetterParameters.GetParameters(setter, 1);
	CustomFlows.AddFlow(oneParam, zeroParam);
	zeroParams.Add(zeroParam);
	
}

Lightning_Map_Component_To_Controller();
Dictionary<string,string> controllerToComponent =
	querySharedData.GetSharedData("Lightning_Cont_To_Cmp") as Dictionary<string,string>;

foreach(CxList setter in setters)
{
	LinePragma setterPragma = setter.GetFirstGraph().LinePragma;
	if(setterPragma.FileName == null)
	{
		continue;
	}	
	string fileName = setterPragma.FileName;
	if(controllerToComponent.ContainsKey(fileName)){	
		string componentName = controllerToComponent[fileName];	
		CxList allexpressionsInCmp = Lightning_Find_All_Expr_In_File(componentName);		
		CxList allDescOfExpressions = cxXPath.GetAllExpressionDescendents(allexpressionsInCmp, 8);
		CxList curSetterZeroParam = zeroParams.GetParameters(setter);
		if(curSetterZeroParam.GetName().StartsWith("v."))
		{			
			string withoutV = curSetterZeroParam.GetName().Remove(0, 2);
			foreach(CxList sink in allDescOfExpressions)
			{
				if(sink.GetName().Equals(withoutV))
				{
					CustomFlows.AddFlow(curSetterZeroParam, sink);				
				}
			}
		}
	}
}