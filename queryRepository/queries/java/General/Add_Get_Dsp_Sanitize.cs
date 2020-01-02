// this query get one parameter -> list of beam names that should be not vulnerable

if (param.Length == 1)
{
	List<string> sanitizerName = param[0] as List<string>;

	CxList methodInvokes = Find_Methods();

	// calculate "get Attribute" and "get Parameter"
	CxList getAttrib = methodInvokes.FindByShortName("getAttribute");
	getAttrib.Add(methodInvokes.FindByShortName("getParameter"));

	// calculate "set Attribute" and "set Parameter"
	CxList setAttrib = 	methodInvokes.FindByShortName("setAttribute");
	setAttrib.Add(methodInvokes.FindByShortName("setParameter"));
		
	setAttrib = All.GetParameters(setAttrib);
	
	CxList classDecls = Find_Class_Decl();
	CxList beanSanitizers = All.NewCxList();
	
	foreach (string className in sanitizerName)
	{
		beanSanitizers.Add(classDecls.FindByShortName(className));
	}

	CxList sanitizers = getAttrib.GetByAncs(beanSanitizers);
	sanitizers.Add(setAttrib.GetByAncs(beanSanitizers));
	
	result = sanitizers;
}