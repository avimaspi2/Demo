CxList obj = Find_Object_Create().FindByShortName("File*");
obj.Add(All.FindByMemberAccess("ReadCert.getKeyInfo"));

CxList strings = Find_Strings();

CxList linuxStrings = strings.FindByShortName("*/*");
CxList windowsStrings = strings.FindByShortName("*\\\\*");
CxList strings_with_separator = linuxStrings + windowsStrings;

//remove invalid string separators
List < string > invalidLinux = new List<string> {"*<*","*>*","*;*","*\\n","*=*","*http:*","*https:","*?*","*\\*"};
List < string > invalidWindows = new List<string> {"*<*","*>*","*;*","*\\n","*=*","*http:*","*https:","*?*","***","*/*"};
CxList invalidLinuxStrings = linuxStrings.FindByShortNames(invalidLinux);
CxList invalidWindowsStrings = windowsStrings.FindByShortNames(invalidWindows);

CxList invalidSeparators = All.NewCxList();
invalidSeparators.Add(invalidLinuxStrings);
invalidSeparators.Add(invalidWindowsStrings);

strings_with_separator -= invalidSeparators;

CxList htmlCreationMethodInvokes = Find_Html_Outputs(); 
CxList htmlCreationParams = All.GetParameters(htmlCreationMethodInvokes);
CxList htmlSetSrcParams = All.GetParameters(htmlCreationMethodInvokes.GetMembersOfTarget().FindByShortName("setSrc"));

// HTML elements should receive forward slash only, there's no forward/backward slash portability issue here.
strings_with_separator -= htmlCreationParams;
strings_with_separator -= htmlSetSrcParams;

CxList replace = All.FindByShortName("replace*", false);
replace = replace.FindByType(typeof(MemberAccess)) + 
	replace.FindByType(typeof(MethodInvokeExpr));

CxList replace_string = All.GetParameters(replace, 0).FindByType(typeof(StringLiteral));
CxList replace_with_separator = strings_with_separator * replace_string;

// exclude the strings with separators that appear as first argument in "replace"
strings_with_separator -= strings_with_separator.GetByAncs(replace_with_separator);

//find variables of string with seperator
CxList variablesOfStringWithSeparator = strings_with_separator.GetAssignee(All);
//find all references of string with separator(it will find references inside of a setter)
CxList referencesOfVariables = All.FindAllReferences(variablesOfStringWithSeparator);
referencesOfVariables -= All.FindDefinition(referencesOfVariables);
//it will be only used to find setters after we should remove them in order to have
//more readable flows
strings_with_separator.Add(referencesOfVariables);

CxList methods = Find_Methods();
CxList sanitize = Find_Integers();
sanitize.Add(methods.FindByShortName("getClass"));

List <string> setterMethodsNames = new List<string>(){"setParameter", "setProperty", "setAttribute"};
List <string> getterMethodsNames = new List<string>(){"getParameter", "getProperty", "getAttribute"};
//find setters that has some param related with separators
CxList setterMethods = strings_with_separator.GetAncOfType(typeof(MethodInvokeExpr)).FindByShortNames(setterMethodsNames);
strings_with_separator -= referencesOfVariables;

CxList constants = Find_Constants();

CxList getterMethods = methods.FindByShortNames(getterMethodsNames);

CxList settersFirstParam = All.GetParameters(setterMethods, 0);
	
//String literal params
CxList setterStringParam = settersFirstParam.FindByType(typeof(StringLiteral));
	
CxList unknownReferenceSetterParam = settersFirstParam.FindByType(typeof(UnknownReference));
unknownReferenceSetterParam.Add(settersFirstParam.FindByType(typeof(MemberAccess)));
//find variable in param that are constant
CxList definitionSetterParam = All.FindDefinition(unknownReferenceSetterParam).GetByAncs(constants);
	
CxList setterFirstParamByType = setterStringParam;
setterFirstParamByType.Add(definitionSetterParam);

//getters that has influence on outputs
CxList gettersInfluencingOnOutputs = getterMethods.InfluencingOn(obj);
gettersInfluencingOnOutputs = gettersInfluencingOnOutputs.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);

//the first parameter of setter should influence the getter
CxList gettersInfluencedBySetterParameter = gettersInfluencingOnOutputs.InfluencedBy(setterFirstParamByType);

result = strings_with_separator.InfluencingOnAndNotSanitized(obj, sanitize).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);

List<Tuple<CxList,CxList>> interestingSetters_Getters = new List<Tuple<CxList,CxList>>();

foreach(CxList setterToGetter in gettersInfluencedBySetterParameter.GetCxListByPath()){
	CxList getter = setterToGetter.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
	CxList getterFirstParam = All.GetParameters(getter, 0);
	CxList setterFirstParam = setterToGetter.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
	CxList setter = setterFirstParam.GetAncOfType(typeof(MethodInvokeExpr));
		
	CSharpGraph setterParam = setterFirstParam.TryGetCSharpGraph<CSharpGraph>();
	CSharpGraph getterParam = getterFirstParam.TryGetCSharpGraph<CSharpGraph>();
		
	//Calculate if first param of setter is the same as getter
	if(setterParam != null && getterParam != null )
	{	
		if(setterParam.ShortName.Trim('"') == getterParam.ShortName.Trim('"'))
		{
			Tuple<CxList,CxList> t = Tuple.Create(setter, getter);
			interestingSetters_Getters.Add(t);
		}
		else{
			Tuple<CxList,CxList> t = Tuple.Create(setter, All.NewCxList());
			interestingSetters_Getters.Add(t);	
		}
	}
}

CxList toRemove = All.NewCxList();
CxList notRemove = All.NewCxList();

foreach(Tuple<CxList,CxList> pair in interestingSetters_Getters){
	CxList setter = pair.Item1;
	CxList getter = pair.Item2;
	CSharpGraph setterGraph = setter.TryGetCSharpGraph<CSharpGraph>();
	string setterName = setterGraph != null ? setterGraph.ShortName : null;
	CSharpGraph getterGraph = getter.TryGetCSharpGraph<CSharpGraph>();
	string getterName = getterGraph != null ? getterGraph.ShortName : null;
	
	CxList l1 = result.IntersectWithNodes(All.GetParameters(setter, 1));
	CxList l2 = result.IntersectWithNodes(getter);
	
	//if we have a setter and getter and is the same fuction eg.(setParameter == getParameter)
	//then we will check if both are contained in the previous result calculation then
	//we should not remove them
	//else if we only have the setter the result should be removed
	if(setterName != null && getterName != null && setterName.EndsWith(getterName.Replace("get", ""))){
		if(l1.Count > 0 && l2.Count > 0){
			notRemove.Add(l1);	
		}
	}
	else{
		if(l1.Count > 0 && l2.Count == 0){
			toRemove.Add(l1);
		}
	}
}
toRemove = toRemove - (toRemove * notRemove);

//remove flows that only have the setter
result -= toRemove;