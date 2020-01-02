if(ReactNative_Find_Presence().Count > 0){
	CxList objectCreations = Find_ObjectCreations();
	CxList fieldDecls = Find_FieldDecls();
	CxList booleanLiterals = Find_BooleanLiteral();

	//Look for TextInput of sensitive/personal information
	CxList textInput = objectCreations.FindByShortName("TextInput");
	CxList personalInfo = Find_Personal_Info();
	CxList personalInfoTextInput = textInput.FindByParameters(personalInfo);

	foreach(CxList piti in personalInfoTextInput)
	{
		CxList ste = fieldDecls.GetByAncs(piti).FindByShortName("secureTextEntry");
		if (ste.Count == 0)
		{
			//When the secureTextEntry flag is absent, show the Component
			result.Add(piti);
		}
		else
		{
			//When the secureTextEntry flag is set to false, show the flag
			CxList falses = booleanLiterals.GetByAncs(ste).FindByShortName("false");
			result.Add(falses.GetAncOfType(typeof(FieldDecl)) * ste);
		}
	}
}