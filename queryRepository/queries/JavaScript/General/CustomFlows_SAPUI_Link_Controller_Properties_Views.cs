// XML
//Get the Cx Xml Config Classes
CxList methodDecls = Find_MethodDecls();
CxList memberAccesses = Find_MemberAccesses();
CxList methods = Find_Methods();
CxList cxXmlConfigClasses = methodDecls.FindByShortName("CxXmlConfigClass*");

CxList getProperties = methods.FindByShortName("getProperty");
CxList setProperties = methods.FindByShortName("setProperty");
CxList getModels = methods.FindByShortName("getModel");
CxList getModelsParameters = All.GetParameters(getModels);
CxList setPropertiesParameters = All.GetParameters(setProperties);
CxList parameters = Find_Param();

Dictionary<String, CxList> mapViewController = new Dictionary<String, CxList>();
CxList controllerNameInView = memberAccesses.FindByShortName("controllername", false);

// Iterates over all CxXmlConfigClass classes
foreach(CxList cxXmlConfigClass in cxXmlConfigClasses)
{
	CxList views = Find_UnknownReference().GetByAncs(cxXmlConfigClass).FindByShortName("mvc_view", false);
	CxList controllers = controllerNameInView.FindByFathers(views)
		.GetAssigner()
		.FindByType(typeof(StringLiteral)); 
	
	if(controllers.Count == 0) 
		continue;
	
	// Creates a dictionary (key: controllerName, value: CxXmlConfigClass
	string controllerName = controllers.GetName();
	if(mapViewController.ContainsKey(controllerName))
	{
		mapViewController[controllerName].Add(cxXmlConfigClass);
	}
	else
	{
		mapViewController.Add(controllerName, cxXmlConfigClass.Clone());
	}
}

//Gets all the controller imports
CxList controllerImports = Find_SAPUI_Extending_Objects("sap/ui/core/mvc/Controller");

// Iterates over all Controllers
foreach(CxList controllerImport in controllerImports)
{
	//Gets the controller name
	CxList controllerName = All.GetParameters(controllerImport.GetAncOfType(typeof(MethodInvokeExpr)), 0)
		.FindByType(typeof(StringLiteral));
	
	//Validates that the controller name is a StringLiteral
	if(controllerName.Count == 0) continue;	
	
	string controllerNameString = controllerName.GetName();
	
	//Ensures dictionary contains the controller
	if(!mapViewController.ContainsKey(controllerNameString)) continue;
	
	//Tries to get the CxXmlConfigClass for the controller
	CxList viewXMLMethod = mapViewController[controllerNameString];
			
	//Validates that the controller name is a StringLiteral
	if(viewXMLMethod.Count == 0) continue;
	
	//Gets all the GetProperty & GetModel methods that descendants of the CxXmlConfigClass
	CxList viewDescendantsGetProperties = getProperties.GetByAncs(viewXMLMethod);
	
	//Gets all the SetProperty & GetModel methods that descendants of the controller
	CxList controllerDescendantGetModels = getModels.GetByAncs(controllerImport);
	CxList controllerDescendantsSetProperties = setProperties.GetByAncs(controllerImport);

	// Iterates over GetProperty methods that are descendant of the cxXmlConfigClass
	foreach(CxList viewDescendantGetProperty in viewDescendantsGetProperties)
	{
		CxList getPropertyName = All.GetParameters(viewDescendantGetProperty, 0);
		CxList getModelName = All.GetParameters(viewDescendantGetProperty.GetTargetOfMembers(), 0);
		
		// Gets the getModel that are descedant of the controller methods 
		// with the same pararameter has the iterated getProperty (descendant of the cxXmlConfigClass).
		CxList getModelsInController = controllerDescendantGetModels
			.FindByParameters(getModelsParameters.FindByShortName(getModelName.GetName()));
		CxList setPropertiesWithSameName = controllerDescendantsSetProperties
			.FindByParameters(setPropertiesParameters.FindByShortName(getPropertyName.GetName()));
		
		CxList correspondantSetProperties = setPropertiesWithSameName * getModelsInController.GetMembersOfTarget();
		
		//Creates the CustomFlows
		foreach(CxList correspondantSetProperty in correspondantSetProperties)
		{
			CxList source = All.GetParameters(correspondantSetProperty, 1) - parameters;
			CustomFlows.AddFlow(source, getPropertyName);
		}
	}
}