if (param.Length == 2)
{
	CxList getAttributeMethods = param[0] as CxList;
	CxList setAttributeMethods = param[1] as CxList;

	CxList allStrInt = All.FindByAbstractValue(abstractValue => (abstractValue is StringAbstractValue ||
		                                                         abstractValue is IntegerIntervalAbstractValue));
		
	CxList constants = Find_Constants();
	
	CxList getAttributeFirstParameterStringsAndIntegers = allStrInt.GetParameters(getAttributeMethods, 0);//Keys for get of type String or Int
	CxList getAttributeFirstParameterConstants = All.GetParameters(getAttributeMethods, 0).FindAllReferences(constants);
	CxList setAttributeSecondParameters = All.GetParameters(setAttributeMethods, 1);  	//Values for set/put
	CxList setAttributeFirstParameters = All.GetParameters(setAttributeMethods, 0);
	
	char[] trimChars = new char[6] {' ', '\t', '"', '(', '\r', '\n'};

	foreach(CxList setAttributeSecondParameter in setAttributeSecondParameters.GetCxListByPath())               //setAttributeSecondParameter may be a path
	{
		//find setAttribute of second Parameter 
		CxList secondParameterSetAttributeEndNode = setAttributeSecondParameter.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
		CxList setAttributeMethodInvoke = secondParameterSetAttributeEndNode.GetAncOfType(typeof(MethodInvokeExpr));
		try
		{
			CSharpGraph setOrPut = setAttributeMethodInvoke.TryGetCSharpGraph<CSharpGraph>();
			string setPutName = setOrPut.ShortName.Trim(trimChars);
			int count = 3;
			bool ifSetPut = false;
			if (setPutName.Equals("setAttribute") == true || setPutName.Equals("put") == true)
				ifSetPut = true;
			while (count > 0 && ifSetPut == false)
			{
				count--;
				setAttributeMethodInvoke = setAttributeMethodInvoke.GetFathers();    //get the parameter representation of second parameter
				setAttributeMethodInvoke = setAttributeMethodInvoke.GetAncOfType(typeof(MethodInvokeExpr));   //get setAttribute/put methods
				CSharpGraph setOrPut1 = setAttributeMethodInvoke.TryGetCSharpGraph<CSharpGraph>();
				if (setOrPut1 == null)
				{
					continue;
				}
				setPutName = setOrPut1.ShortName.Trim(trimChars);
				if (setPutName.Equals("setAttribute") == true || setPutName.Equals("put") == true)
					ifSetPut = true;
			}
		}
		catch (Exception ex)
		{
			cxLog.WriteDebugMessage(ex);
		}
        
		CxList matchingGettersParameters = All.NewCxList();
		CxList currParam = allStrInt.GetParameters(setAttributeMethodInvoke, 0); //get the key parameter of setAttribute
		if(currParam.Count > 0) //We can use the Abstract Interpretation mechanisem
		{
			matchingGettersParameters = getAttributeFirstParameterStringsAndIntegers.FindByAbstractValues(currParam);
		}
		else  //We need to check if it is a constant
		{
			currParam = setAttributeFirstParameters.GetParameters(setAttributeMethodInvoke, 0).FindAllReferences(constants);
			CSharpGraph currParamGraph = currParam.TryGetCSharpGraph<CSharpGraph>();
			if(currParamGraph != null)
			{
				string name = currParamGraph.ShortName.Trim(trimChars);
				matchingGettersParameters = getAttributeFirstParameterConstants.FindByShortName(name);
			}
		}                                                     
		// Connect matching set/put to get
		foreach(CxList getAttributeMethodInvoke in matchingGettersParameters.GetAncOfType(typeof (MethodInvokeExpr)))
		{
			CustomFlows.AddFlow(setAttributeSecondParameter, getAttributeMethodInvoke);
		}		
	}
}