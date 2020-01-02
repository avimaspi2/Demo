if(cxScan.IsFrameworkActive("SAPUI")) 
{
	CxList thisRef = Find_ThisRef();
	CxList parameters = Find_Param();
	CxList paramDecl = Find_ParamDecl();
	CxList fieldDecls = Find_FieldDecls();
	CxList lambdaExpr = Find_LambdaExpr();
	CxList unknownReferences = Find_UnknownReference();

	// find control extend => Control.extend(...);
	CxList controls = Find_SAPUI_Extending_Objects("sap/ui/core/Control");
	
	// find the control in the 'renderer' and 'render' function (second parameter) => renderer: function(oRm, oControl)
	CxList rendererFieldDecls = fieldDecls.FindByShortNames(new List<string> { "renderer", "render" });
	CxList controlRenderLambda = lambdaExpr.GetByAncs(rendererFieldDecls);
	CxList oControlParameters = paramDecl.GetParameters(controlRenderLambda, 1);

	// find all control references
	CxList controlRef = thisRef.GetByAncs(controls);
	controlRef.Add(unknownReferences.FindAllReferences(controlRef.GetAssignee()));
	controlRef.Add(unknownReferences.FindAllReferences(oControlParameters.GetByAncs(controls)));
	
	foreach(CxList control in controls)
	{	
		// find all 'this' references in this control
		CxList controlReferences = controlRef.GetByAncs(control);
	
		// find 'getProperty' name parameteter => .getProperty(propertyName);
		CxList getPropertyControls = controlReferences.GetMembersOfTarget().FindByShortName("getProperty");
		CxList getPropertyName = All.GetParameters(getPropertyControls, 0) - parameters;
	
		// find 'setProperty' methods => .setProperty(propertyName, value, bool);
		CxList setPropertyControls = controlReferences.GetMembersOfTarget().FindByShortName("setProperty");
	
		foreach (CxList setPropertyControl in setPropertyControls)
		{
			// find 'setProperty' name and value parameter
			CxList setPropertyName = All.GetParameters(setPropertyControl, 0) - parameters;
			CxList setPropertyValue = All.GetParameters(setPropertyControl, 1) - parameters;
		
			// get the current 'getProperty' name
			CxList currentGetPropertyName = getPropertyName.FindByShortName(setPropertyName.GetName());
		
			// add flow between 'getProperty' and 'setProperty' with the same property name
			CustomFlows.AddFlow(setPropertyValue, currentGetPropertyName);
		}
	}
}