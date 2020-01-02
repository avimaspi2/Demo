if(cxScan.IsFrameworkActive("SAPUI"))
{
	CxList parameters = Find_Param();

	CxList thisRefs = Find_ThisRef();
	
	CxList controllers = Find_SAPUI_Extending_Objects("sap/ui/core/mvc/Controller");

	//UiComponents
	CxList uiComponents = Find_SAPUI_Extending_Objects("sap/ui/core/UIComponent");
	CxList componentReferences = thisRefs.GetByAncs(uiComponents);
	componentReferences.Add(All.FindAllReferences(componentReferences.GetAssignee()));

	//Controller
	CxList controllerReference = thisRefs.GetByAncs(controllers);
	controllerReference.Add(All.FindAllReferences(controllerReference.GetAssignee()));

	//getView()
	CxList getViewOnRouteHandling = controllerReference.GetMembersOfTarget().FindByShortName("getView");
	getViewOnRouteHandling.Add(All.FindAllReferences(getViewOnRouteHandling.GetAssignee()));

	//getModel()
	CxList getModelOnRouteHandling = getViewOnRouteHandling.GetMembersOfTarget().FindByShortName("getModel");
	getModelOnRouteHandling.Add(All.FindAllReferences(getModelOnRouteHandling.GetAssignee()));
	getModelOnRouteHandling.Add(componentReferences.GetMembersOfTarget().FindByShortName("getModel"));

	Dictionary<string, CxList> modelsByName = new Dictionary<string, CxList>();

	foreach(CxList model in getModelOnRouteHandling)
	{
		CxList modelParameter = All.GetParameters(model, 0) - parameters;
		string modelName = modelParameter.GetName();

		if(modelsByName.ContainsKey(modelName))
		{
			modelsByName[modelName].Add(model);
		}
		else
		{
			modelsByName.Add(modelName, model.Clone());
		}	
	}

	foreach(KeyValuePair<string, CxList> getModelPair in modelsByName)
	{
		CxList currentModel = getModelPair.Value;

		//getProperty()
		CxList getPropertyOnRouteHandling = currentModel.GetMembersOfTarget().FindByShortName("getProperty");

		//setProperty()
		CxList setPropertyOnRouteHandling = currentModel.GetMembersOfTarget().FindByShortName("setProperty");

		CxList getPropertyParameters = All.GetParameters(getPropertyOnRouteHandling, 0) - parameters;

		foreach (CxList setProperty in setPropertyOnRouteHandling)
		{
			CxList setPropertyParameters = All.GetParameters(setProperty, 0) - parameters;
			CxList setPropertyParametersValue = All.GetParameters(setProperty, 1) - parameters;
	
			CxList currentGetPropertyParameter = getPropertyParameters.FindByShortName(setPropertyParameters.GetName());		
	
			CustomFlows.AddFlow(setPropertyParametersValue, currentGetPropertyParameter);	
		}
	}
}