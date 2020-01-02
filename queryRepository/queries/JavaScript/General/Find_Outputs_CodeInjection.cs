CxList methods = Find_Methods();
CxList unknownRefs = Find_UnknownReference();
CxList lambdaExpressions = Find_LambdaExpr(); 
CxList memberAccesses = Find_MemberAccesses();
CxList paramDecls = Find_ParamDecl();

CxList exec = All.FindByMemberAccess("*.execScript");
exec.Add(Find_Members("window.execCommand"));
exec.Add(Find_Members("document.execCommand"));
exec.Add(methods.FindByShortNames(new List<string>{"setInterval", "setTimeout", "setImmediate"}));

//Recognizing tinymce package execCommand
exec.Add(Find_Members("tinyMCEPopup.execCommand"));
exec.Add(Find_Members("tinymce.execCommand"));
exec.Add(Find_Members("tinyMCEPopup.executeOnLoad"));

CxList membersWithSecondParamToSearch = Find_Members("tinymce.execInstanceCommand");
CxList commandParameters = All.NewCxList();
commandParameters.Add(All.GetParameters(exec, 0));
commandParameters.Add(All.GetParameters(membersWithSecondParamToSearch, 1));	
CxList onlyParams = commandParameters.FindByType(typeof(Param)); 
CxList relevantParameters = commandParameters - onlyParams - lambdaExpressions; // <- avoid anonymous methods to be considered code injection
CxList invoke = onlyParams.GetAncOfType(typeof(MethodInvokeExpr));
CxList evalParam = relevantParameters.GetByAncs(onlyParams);
result.Add(evalParam.ConcatenateAllTargets(invoke, true));

// Add "eval" members
CxList evalAppearences = All.NewCxList();
evalAppearences.Add(methods, unknownRefs);
CxList evalMembers = Find_Members("eval") * evalAppearences;
result.Add(evalMembers);

// Add "eval" aliases
CxList possibleAliases = All.NewCxList();
possibleAliases.Add(unknownRefs, paramDecls, memberAccesses);
CxList evalReferences = evalMembers.FindByType(typeof(UnknownReference));
CxList evalAliases = possibleAliases.InfluencedBy(evalReferences);
evalAliases.Add(evalReferences.GetAssignee());
result.Add(methods.FindAllReferences(evalAliases));

//Add new Function last parameter as an output.
CxList functionObject = All.FindByShortName("CxEval").GetAncOfType(typeof(IfStmt));
CxList childrenOfIf = All.GetByAncs(functionObject);
result.Add(childrenOfIf);

//Add source elements as outputs
CxList sourceElements = Find_String_Short_Name(Find_String_Literal(), "'script'", false);
sourceElements = Find_Members("*.createElement").FindByParameters(sourceElements);
sourceElements = unknownRefs * unknownRefs.DataInfluencedBy(sourceElements);
sourceElements.Add(All.FindAllReferences(sourceElements));
sourceElements = sourceElements.GetMembersOfTarget();
result.Add(sourceElements.FindByShortNames(
	new List<string>{"src",
		"text", 			//Relevant to Explorer
		"textContent", 		//Relevant to all but Explorer
		"innerText"			//Relevant to all but Firefox
		}));

//Add on Event method calls as outputs
CxList eventNames = memberAccesses * Find_OnEvents();
eventNames -= eventNames.GetMembersOfTarget().GetTargetOfMembers();
result.Add(eventNames);

CxList setlist = Find_Vulnerable_SetAttribute();
CxList nonSetList = setlist.FindByShortName("ready");
nonSetList.Add(Find_JQuery_Event());
setlist -= nonSetList;

result.Add(setlist);

//add Proxy(eval,{}) as an output
CxList eval = All.FindByShortName("eval", false).FindByType(typeof(Param));
CxList proxy = All.FindByParameters(eval).FindByShortName("Proxy", false);
CxList proxyAlias = proxy.GetAssignee();
CxList proxyInvoke = methods.FindAllReferences(proxyAlias);
result.Add(proxyInvoke);

result.Add(All.FindByMemberAccess("script.src"));
result.Add(Find_Members("script.src"));
result.Add(Find_JQuery_Outputs_CodeInjection());
result.Add(Find_MsAjax_Outputs_CodeInjection());

CxList ngClick = methods.FindByShortName("ng_Click");
result.Add(ngClick);