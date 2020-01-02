CxList source = Find_Methods();
source.Add(Find_MethodRef());

CxList strings = Find_String_Literal();

strings -= strings.FindByName("\"\"");

CxList allStrInt = Find_IntegerLiterals();

allStrInt.Add(strings);

CxList objects = source.FindByName("*cookieStore.*");
objects.Add(source.FindByName("*$templateCache.*"));

CxList objectsPut = objects.FindByName("*cookieStore.put");
objectsPut.Add(objects.FindByName("*$templateCache.put"));

CxList objectsGet = objects.FindByName("*cookieStore.get");
objectsGet.Add(objects.FindByName("*$templateCache.get"));


CxList secondParam = All.GetParameters(objectsPut, 1);		//Values for put
CxList getStrings = strings.GetParameters(objectsGet, 0);   //Keys for get 

char[] trimChars = new char[6] {' ', '\t', '"', '(', '\r', '\n'};
CxList setAttrConcatinated = All.NewCxList();

foreach(CxList setA in secondParam.GetCxListByPath())               //setA may be a path
{
	CxList setALast = All.NewCxList();
	CxList getInput = All.NewCxList();

	//find setAttribute of second Parameter 
	CxList secParamSetAttrOrig = setA.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
	CxList secParamSetAttr = secParamSetAttrOrig.GetAncOfType(typeof(MethodInvokeExpr));

	try
	{
		CSharpGraph setOrPut = secParamSetAttr.GetFirstGraph();
		string setPutName = setOrPut.ShortName.Trim(trimChars);
		int count = 3;
		bool ifSetPut = false;
		if (setPutName.Equals("put") == true)
			ifSetPut = true;
		while (count > 0 && ifSetPut == false)
		{
			count++;
			secParamSetAttr = secParamSetAttr.GetFathers();                             //get the parameter representation of second parameter
			secParamSetAttr = secParamSetAttr.GetAncOfType(typeof(MethodInvokeExpr));   //get put methods

			CSharpGraph setOrPut1 = secParamSetAttr.GetFirstGraph();
			if (setOrPut1 == null)
			{
				continue;                            
			}
			setPutName = setOrPut1.ShortName.Trim(trimChars);
			if (setPutName.Equals("put") == true)
				ifSetPut = true;
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
                                
	try
	{
		//Part1: strings/int keys 
		CxList currParam = allStrInt.GetParameters(secParamSetAttr, 0); //get the key parameter of setAttribute
         
		CSharpGraph gr = currParam.GetFirstGraph();
		if (currParam.Count > 0 )
		{
			string name = gr.ShortName.Trim(trimChars);
			getInput = getStrings.FindByShortName(name);  // search matching of get
		}
		//********************************************************
	}

	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
                                
	//concatenate matching set/put to get
	setALast = setA.ConcatenateAllPaths(getInput.GetAncOfType(typeof (MethodInvokeExpr)), false);
	setAttrConcatinated.Add(setALast);
}

result = setAttrConcatinated;