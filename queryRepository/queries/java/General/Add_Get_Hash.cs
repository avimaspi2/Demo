CxList hash = All.FindByType("Hashtable"); 
hash.Add(All.FindByType("Collection")); 
hash.Add(All.FindByType("Map"));

CxList putHash = hash.GetMembersOfTarget().FindByShortName("put");
CxList getHash = hash.FindAllReferences(putHash.GetTargetOfMembers()).GetMembersOfTarget().FindByShortName("get");

CxList getAttrib = All.NewCxList();
getAttrib.Add(getHash);

CxList setAttrib = All.NewCxList();
setAttrib.Add(putHash);

CxList integerLiterals = Find_IntegerLiterals();
CxList allStrings = Find_Strings() - Find_Empty_Strings();
CxList allConstants = Find_Constants();
CxList allStrInt = All.NewCxList();
allStrInt.Add(allStrings);

allStrInt.Add(Find_Integers());
allStrInt.Add(integerLiterals);

CxList secondParam = All.GetParameters(setAttrib, 1);  	//Values for set/put
CxList firstParameters = All.GetParameters(setAttrib, 0);  	//Keys for set/put            

CxList allGetParameters = All.GetParameters(getAttrib, 0);  //Keys for get of any type
	//********************************************************

char[] trimChars = new char[6] {' ', '\t', '"', '(', '\r', '\n'};

foreach(CxList setA in secondParam.GetCxListByPath())               //setA may be a path
{
	CxList getInput = All.NewCxList();
                                
	//find setAttribute of second Parameter 
	CxList secParamSetAttrOrig = setA.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
	CxList secParamSetAttr = secParamSetAttrOrig.GetAncOfType(typeof(MethodInvokeExpr));
	try{
		CSharpGraph setOrPut = secParamSetAttr.TryGetCSharpGraph<CSharpGraph>();
		string setPutName = setOrPut.ShortName.Trim(trimChars);
		int count = 3;
		bool ifSetPut = false;

		if (setPutName.Equals("put"))
			ifSetPut = true;
		while (count > 0 && ifSetPut == false)
		{
			count--;

			secParamSetAttr = secParamSetAttr.GetFathers();                             //get the parameter representation of second parameter
			secParamSetAttr = secParamSetAttr.GetAncOfType(typeof(MethodInvokeExpr));   //get setAttribute/put methods
			setOrPut = secParamSetAttr.TryGetCSharpGraph<CSharpGraph>();
			setPutName = setOrPut.ShortName.Trim(trimChars);
			if (setPutName.Equals("put"))
				ifSetPut = true;
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
	
	CxList currParam = allStrInt.GetParameters(secParamSetAttr, 0); //get the key parameter of setAttribute
	CSharpGraph gr = currParam.TryGetCSharpGraph<CSharpGraph>();
	CxList currParamGet = hash.FindAllReferences(currParam.GetAncOfType(typeof (MethodInvokeExpr)).GetTargetOfMembers()).GetMembersOfTarget().FindByShortName("get");
	CxList currGetParams = allGetParameters.GetParameters(currParamGet, 0);
	bool isConst = false;        //indicates the type of key: string/int or const
	

	if (currParam.Count == 0){           
		//setAttribute/get of hash parameter is not StringLiteral
		currParam = firstParameters.GetParameters(secParamSetAttr, 0).FindAllReferences(allConstants);
		gr = currParam.TryGetCSharpGraph<CSharpGraph>();
		isConst = true;   //key type is const
	}

	
	if(currParam.Count > 0){
		string name = gr.ShortName.Trim(trimChars);
		if (isConst == true){
			getInput = currGetParams.FindByShortName(name);
		}
		else{
			getInput = currGetParams.FindByShortName("\"" + name + "\"");
			if (getInput.Count == 0){
				//the key is of type integer/integerliteral
				getInput = currGetParams.FindByShortName(name);
			}
		}
	}

	// Connect matching set/put to get
	foreach(CxList g in getInput.GetAncOfType(typeof (MethodInvokeExpr)))
	{
		CustomFlows.AddFlow(setA, g);
	}
}