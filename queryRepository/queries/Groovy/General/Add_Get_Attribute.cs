/*
	Find all paths to getAttribute for sessions or get for Map/List/HashTable that influenced by input
The query has 3 parts:
1. param.Length == 1: is an interface for other queries
2. param.Length == 2: 
		first parameter is an input 
		second parameter indicates iteration number (currently up to 5 iterations)
3. param.Length == 5: 
		parameter1 is input, 
		parameter2 is get methods of hashes, maps or sessions, 
		parameter3 is all set/put methods for sessions/hashes,maps,
		parameter4 indicates current iteration, 
		parameter5 indicates input origin – session or hashes/maps 

*/
if (param.Length == 1)
{
	CxList allInputs = param[0] as CxList;
	result.Add(Add_Get_Attribute(allInputs, "0"));
}
		
else if (param.Length == 2){
	//second parameter indicates current iteration
	/*
	Part1: add/get to session 
	Part2: put/get to Hashtable/Map/ Collection
	Part3: add/get to List
	*/
	CxList allInputs = param[0] as CxList;
	string currIteration = param[1] as String;

	//Part1
	//set and get to/from session
	result = 
		Add_Get_Attribute(allInputs, Get_Session_Attribute(), Set_Session_Attribute(), currIteration, "session") +
		Add_Get_Attribute(allInputs, Get_Context_Attribute(), Set_Context_Attribute(), currIteration, "session");
	
	//Part2
	//Add hashed inputs influenced by the input
	CxList hash = 
		All.FindByType("Hashtable") + 
		All.FindByType("Collection") + 
		All.FindByType("Map");

	CxList putHash = hash.GetMembersOfTarget().FindByShortName("put");// +hash.GetMembersOfTarget().FindByShortName("putAll");
	CxList getHash = hash.FindAllReferences(putHash.GetTargetOfMembers()).GetMembersOfTarget().FindByShortName("get");
	
	//add and get to/from hash structures
	result.Add(Add_Get_Attribute(allInputs, getHash, putHash, currIteration, "hash"));

	
	//Part3 
	//Adds paths to get method of Lists
	
	CxList allDeclAndFieldDecl = All.FindByType(typeof(FieldDecl)) + All.FindByType(typeof(Declarator));
	CxList allUnknownRefAndParams = All.FindByType(typeof(ParamDecl)) + All.FindByType(typeof(UnknownReference));
	CxList allLists = allUnknownRefAndParams.FindAllReferences(allDeclAndFieldDecl.FindByRegex(@"List\s*<"));

	CxList allAdds = allLists.GetMembersOfTarget().FindByShortName("add") + allLists.GetMembersOfTarget().FindByShortName("addAll");

	CxList addGetList = All.NewCxList();

	CxList addsList = 
		allInputs.DataInfluencingOn(allAdds, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);
	foreach(CxList addA in addsList.GetCxListByPath()){
		CxList addAStart = addA.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly).GetTargetOfMembers();
		CxList getList = allLists.FindAllReferences(addAStart).GetMembersOfTarget().FindByShortName("get");
		//Lists with add/allAll methods
	
		//allInputs.DataInfluencingOn(addA, CxList.InfluenceAlgorithmCalculation.NewAlgorithm).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly).GetTargetOfMembers();
		
		//concatinate add/addAll to get
		//addGetList.Add(addA.DataInfluencedBy(allInputs, CxList.InfluenceAlgorithmCalculation.NewAlgorithm).ConcatenateAllPaths(getList));
		addGetList.Add(addA.ConcatenateAllPaths(getList));
	}
	result.Add(addGetList);
	
	int currIntIteration = int.Parse(currIteration);
	if (addGetList.Count > 0 && currIntIteration < 1){
		currIntIteration++;
		Add_Get_Attribute(result, currIntIteration.ToString());
	}
	
}
		
else if (param.Length == 5)
{
	/*
		Concatinate all matching gets to sets/puts for sessions and hashes
		Before entering Parts 1/2/3, Find all Set keys, Get keys, hashes, paths to setAttribute/put methods
		Part1: strings/int keys for session
		Part2: ConstantDecl keys for session
		Part3: keys for hashes (Hashtable/ Collection/Map)
	*/
	CxList allInputs = param[0] as CxList;
	CxList getAttr = param[1] as CxList;
	CxList setAttr = param[2] as CxList;
	string currIteration = param[3] as String;
	string isHash = param[4] as String;		//for hash structure isHash = "hash", for session isHash = "session"

	//path to setAttribute\put methods
	//setAttr = allInputs.DataInfluencingOn(setAttr, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);

	//Find Keys of setAttribute, getAttribute, put and get for hashes
	//********************************************************
	CxList allStrings = Find_Strings() - Find_Empty_Strings();
	CxList allBinaryExpr = All.FindByType(typeof (BinaryExpr));
	CxList allConstants = All.FindByType(typeof(ConstantDecl));
	CxList allStrInt = allStrings + Find_Integers() + All.FindByType(typeof(IntegerLiteral));

	CxList getStrings = allStrInt.GetParameters(getAttr, 0);	//Keys for get of type String or Int
	CxList firstParameters = All.GetParameters(setAttr.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly), 0); //Keys for set/put
	CxList allSecondParameters = All.GetParameters(setAttr, 1);	//Values for set/put

	//values influenced by inputs
	CxList secondParam = allInputs.DataInfluencingOn(allSecondParameters, CxList.InfluenceAlgorithmCalculation.NewAlgorithm)	
		+ allInputs * allSecondParameters;
	
	CxList getConstants = All.GetParameters(getAttr, 0).FindAllReferences(allConstants);	//Keys for get of type ConstantDecl
	CxList allGetParameters = All.GetParameters(getAttr, 0);	//Keys for get of any type
	//********************************************************

	char[] trimChars = new char[6] {' ', '\t', '"', '(', '\r', '\n'};
	CxList setAttrConcatinated = All.NewCxList();
	
	//Find all hashes
	//********************************************************
	CxList hash = 
		All.FindByType("Hashtable") + 
		All.FindByType("Collection") + 
		All.FindByType("Map");
	//********************************************************
	
	foreach(CxList setA in secondParam.GetCxListByPath())	//setA is a path
	{
		//for each path to setAttribute/put that is influenced by input, find matching getAttribute/get
		CxList setALast = All.NewCxList();
		CxList getInput = All.NewCxList();
		
		//find setAttribute of second Parameter 
		CxList secParamSetAttrOrig = setA.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
		CxList secParamSetAttr = secParamSetAttrOrig.GetAncOfType(typeof(MethodInvokeExpr));
		try{
			CSharpGraph setOrPut = secParamSetAttr.data.GetByIndex(0) as CSharpGraph;
			string setPutName = setOrPut.ShortName.Trim(trimChars);
			int count = 3;
			bool ifSetPut = false;
			if (setPutName.Equals("setAttribute") == true || setPutName.Equals("put") == true)
				ifSetPut = true;
			while (count > 0 && ifSetPut == false){
				count--;
				secParamSetAttr = secParamSetAttr.GetFathers();	//get the parameter representation of second parameter
				secParamSetAttr = secParamSetAttr.GetAncOfType(typeof(MethodInvokeExpr));	//get setAttribute/put methods
				setOrPut = secParamSetAttr.data.GetByIndex(0) as CSharpGraph;
				setPutName = setOrPut.ShortName.Trim(trimChars);
				if (setPutName.Equals("setAttribute") == true || setPutName.Equals("put") == true)
					ifSetPut = true;
			}
		}
		catch (Exception ex)
		{
			cxLog.WriteDebugMessage(ex);
		}
		
		try{
			//Part1: strings/int keys for session
			//CxList currParam = allStrInt.GetParameters(setA.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly), 0); //get parameter of setAttribute
			CxList currParam = allStrInt.GetParameters(secParamSetAttr, 0); //get the key parameter of setAttribute
			
			CSharpGraph gr = currParam.data.GetByIndex(0) as CSharpGraph;
			if (currParam.Count > 0 && isHash.Equals("session")){
				string name = gr.ShortName.Trim(trimChars);
				getInput = getStrings.FindByShortName("\"" + name + "\"");
			}
			//********************************************************
			
			//Part2: ConstantDecl keys for session
			bool isConst = false;	//indicates the type of key: string/int or const
			if (currParam.Count == 0){	
				//setAttribute/get of hash parameter is not StringLiteral
				//currParam = firstParameters.GetParameters(setA.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly), 0).FindAllReferences(allConstants);
				currParam = firstParameters.GetParameters(secParamSetAttr, 0).FindAllReferences(allConstants);
				gr = currParam.data.GetByIndex(0) as CSharpGraph;
				isConst = true;	//key type is const
			}
			
			// if still not found, try to find if parameter is affcted by a string literal.
			// The next segment treats the case where the key is a variable that is assigned
			// with a string literal (e.g.: String str = "field1"; setAttribute(str);)
			if(currParam.Count == 0)
			{
				CxList temp = firstParameters.GetParameters(secParamSetAttr, 0);
				CxList source = allStrings.InfluencingOn(temp);
				if(source.Count > 0)
				{		
					CxList otherOcurrences = allStrings.FindByShortName("\"" + source.GetName() + "\"");
					CxList target = getAttr.InfluencedByAndNotSanitized(otherOcurrences, allBinaryExpr)
						.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
					//CxList target = getAttr.InfluencedBy(otherOcurrences) * getAttr;
					getInput = allGetParameters.GetParameters(target, 0);			
				}
			}
			
			if (isConst == true && currParam.Count > 0 && isHash.Equals("session")){
				string name = gr.ShortName.Trim(trimChars);
				getInput = getConstants.FindByShortName(name);
				
				// Following code is added for the case where the set is using a constant as a key,
				// and the get is using a string literal, as follows:
				//
				// public static final String FOO = "foo";
				// String foo = (String) request.getParameter(FOO);
				// request.setAttribute(FOO, foo);
				// response.write((String)request.getAttribute("foo"));
				
				// Find the value of the constant
				CxList val = allStrings.GetByAncs(All.FindDefinition(currParam));
				// If the value is found, find it by name and add to the getInput
				if(val.Count > 0)
				{
					gr = val.data.GetByIndex(0) as CSharpGraph;
					name = gr.ShortName.Trim(trimChars);
					getInput.Add(getStrings.FindByShortName("\"" + name + "\""));
				}
			}
			//********************************************************
			
			//Part3: keys for hashes (Hashtable/ Collection/Map)
			//Find key of get methods for hashes
			CxList currParamGet = hash.FindAllReferences(currParam.GetAncOfType(typeof (MethodInvokeExpr)).GetTargetOfMembers()).GetMembersOfTarget().FindByShortName("get");
			CxList currGetParams = allGetParameters.GetParameters(currParamGet, 0);
			if(isHash.Equals("hash") && currParam.Count > 0){
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
			//********************************************************
		}

		catch (Exception ex)
		{
			cxLog.WriteDebugMessage(ex);
		}
		
		//concatenate matching set/put to get
		setALast = setA.ConcatenateAllPaths(getInput.GetAncOfType(typeof (MethodInvokeExpr)));
		setAttrConcatinated.Add(setALast);
	}
	result = setAttrConcatinated;
	
	//in case current iteration added inputs preform additional iteration up to 5 iterations
	int currIntIteration = int.Parse(currIteration);
	if (setAttrConcatinated.Count > 0 && currIntIteration < 1){
		currIntIteration++;
		result.Add(Add_Get_Attribute(setAttrConcatinated, currIntIteration.ToString()));
	}
}