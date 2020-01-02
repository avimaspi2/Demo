if (param.Length == 1) // param[0] should by all references of UI objects, to be examined if they can be captured by the App switcher
{
	CxList sensitiveWidgets = param[0] as CxList;
	CxList enterBackground = Find_EnterBackground_Statements();
	CxList hidden = Find_Hidden_UI_Widgets();
	
	CxList hiddenInEnterBackground = hidden.GetByAncs(enterBackground);
	
	CxList problematicWidgets = sensitiveWidgets - All.FindAllReferences(hiddenInEnterBackground);
	
	// Find text set to secure (display dots instead of charachters)
	CxList secureTextEntry = Find_Secured_UI_Widgets(problematicWidgets);
	// Find the function under UIApplicationDelegate
	CxList applicationBackground = All.InheritsFrom(@"UIApplicationDelegate");
	CxList delegateEnterBackground = Find_MethodDecls().FindByShortName(@"applicationDidEnterBackground:")
		.GetByAncs(applicationBackground);
	
	// Enabled iOS screenshot capture for sensitive information
	// This part finds personal info UI fields that aren't set as "field.hidden = YES" in a function 
	// that runs before entering background.
	CxList methodsMembers = Find_Methods();
	methodsMembers.Add(Find_MemberAccesses());
	
	CxList hiddenFields = methodsMembers.FindByShortName("*hidden*");
	CxList trulyHidden = Find_BooleanLiteral().FindByFathers(hiddenFields.GetFathers());
	trulyHidden = trulyHidden.FindByShortName("true"); // + trulyHidden.FindByShortName("*YES*"); YES is parsed as true
	hiddenFields = hiddenFields.FindByFathers(trulyHidden.GetFathers());

	CxList hiddenFieldsTarget = hiddenFields.GetTargetOfMembers();
	
	CxList enterAndDelegateBackground = All.NewCxList();
	enterAndDelegateBackground.Add(enterBackground);
	enterAndDelegateBackground.Add(delegateEnterBackground);
	
	hiddenFields = (hiddenFieldsTarget * problematicWidgets).GetByAncs(enterAndDelegateBackground);
	
	// remove secured and hidden text entries
	CxList secureTextFields = All.NewCxList();
	secureTextFields.Add(secureTextEntry);
	secureTextFields.Add(hiddenFields);
	
	result = problematicWidgets - problematicWidgets.FindByShortName(secureTextFields);
}