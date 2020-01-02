if(cxScan.IsFrameworkActive("SAPUI"))
{	
	CxList methods = Find_Methods();
	CxList parameters = Find_Param();
	CxList paramDecl = Find_ParamDecl();
	CxList fieldDecls = Find_FieldDecls();
	CxList lambdaExpr = Find_LambdaExpr();
	CxList sapLibrary = Find_SAP_Library();
	CxList objectCreations = Find_ObjectCreations();
	CxList unknownReferences = Find_UnknownReference();

	// get the controls => sap.ui.core.Control
	CxList controls = Find_SAPUI_Extending_Objects("sap/ui/core/Control");
	
	// get the control 'renderer' function => renderer : function(...)
	CxList rendererFieldDecls = fieldDecls.GetByAncs(controls).FindByShortNames(new List<string> { "renderer", "render" });
	CxList controlRenderLambda = lambdaExpr.GetByAncs(rendererFieldDecls);

	// get render function (oRM) references => renderer : function(oRm, oControl) { ... }
	CxList ormParameters = paramDecl.GetParameters(controlRenderLambda, 0);
	CxList ormReferences = unknownReferences.FindAllReferences(ormParameters);

	// methods with potential vulnerable output parameters
	List<string> potentialVulMethodsNames = new List<string> {
			"write",					// Elements are written to HTML without validation
			"writeAttribute",			// Attributes are written to HTML without validation
			"addClass",					// Classes are written to HTML without validation
			"addStyle",					// Styles are written to HTML without validation
			"writeIcon"					// Attributes and classes are written to HTML without validation
			};

	// methods with potential vulnerable first output parameters
	List<string> potentialVulMethodsNamesFirstParam = new List<string> {
			"writeAttributeEscaped",	// These method do not escape their first parameter
			};

	// get the potential vulnerable methods
	CxList potentialVulMethods = All.NewCxList();
	List<string> allVulMethods = new List<string> {};
	allVulMethods.AddRange(potentialVulMethodsNames);
	allVulMethods.AddRange(potentialVulMethodsNamesFirstParam);
	foreach(CxList potentialVulMethod in methods.FindByShortNames(allVulMethods))
	{
		CxList ormVulMethod = potentialVulMethod.GetLeftmostTarget() * ormReferences;
		if(ormVulMethod.Count > 0) 
			potentialVulMethods.Add(potentialVulMethod);
	}

	// get the vulnerable output parameters
	CxList potentialVulParam = All.GetParameters(potentialVulMethods.FindByShortNames(potentialVulMethodsNames)) - parameters;
	potentialVulParam.Add(All.GetParameters(potentialVulMethods.FindByShortNames(potentialVulMethodsNamesFirstParam), 0) - parameters);

	// get the object creations that instantiate a SAP HTML instance (sap.ui.core.HTML)
	CxList sapLibraryObjCreations = sapLibrary * objectCreations;
	CxList sapHtml = sapLibraryObjCreations.FindByType("sap.ui.core.HTML*");
	CxList sapHtmlReferences = unknownReferences.FindAllReferences(sapHtml.GetAssignee());
	CxList sapHtmlRefMemberTargets = sapHtmlReferences.GetMembersOfTarget();

	// get all the vulnerable HTML output parameters => oHTML.setContent(oTextArea.getValue());
	CxList potentialVulHtmlOutput = All.GetParameters(sapHtmlRefMemberTargets
		.FindByShortNames(new List<string> { "setContent", "setDOMContent" })) - parameters;

	// get all the vulnerable HTML 'content' => oHTML.content = oTextArea.getValue();
	potentialVulHtmlOutput.Add(sapHtmlRefMemberTargets.FindByShortName("content").GetAssigner());

	// result
	result = potentialVulParam;
	result.Add(potentialVulHtmlOutput);

}