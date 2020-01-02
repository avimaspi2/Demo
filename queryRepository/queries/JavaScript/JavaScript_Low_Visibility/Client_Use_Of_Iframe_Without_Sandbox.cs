CxList iframe = All.FindByShortName("iframe__*");

CxList allLiterals = Find_String_Literal();
CxList allSrc = All.FindByShortName("src");
CxList allSandbox = All.FindByShortName("sandbox", false);

foreach(CxList attribute in iframe)
{
	CxList parameter = allLiterals.GetParameters(allSrc.GetByAncs(attribute));
	foreach(CxList str in parameter)
	{	
		if(Find_String_Short_Name(str, ".*", false).Count > 0 || Find_String_Short_Name(str, @"\*", false).Count > 0 ||
			Find_String_Short_Name(str, @"/*", false).Count > 0)
		{		
			continue;
		}
		if(allSandbox.GetByAncs(attribute).Count == 0)
		{
			result.Add(attribute);
		}
	}
}

// element.innerHTML = "<iframe sandbox=allow-same-origin,allow-scripts ...
CxList dom = Find_DOM_Elements();
CxList innerHTMLDyn = dom.GetMembersOfTarget().FindByShortName("innerHTML");
CxList innerHTMLDynValue = innerHTMLDyn.GetAssigner();
innerHTMLDynValue.Add(All.FindByShortName("write"));
// $('<iframe src="' + iframeSrc + '" style="display: none;" />').appendTo('body')
List < string > jqueryAddMethods = new List<string>(){"append*", "prepend*", "add", "insert*"};
innerHTMLDynValue.Add(Find_JQuery_Methods().FindByShortNames(jqueryAddMethods).GetTargetOfMembers());

foreach(CxList attribute in innerHTMLDynValue)
{
	CxList parameter = allLiterals.GetByAncs(attribute);

	if(parameter.FindByShortName("*iframe*", false).Count > 0 && parameter.FindByShortName("*sandbox*").Count == 0)
	{
		result.Add(parameter.FindByShortName("*<iframe*", false));
	}
}

// var element = document.createElement("iframe");
// elem.setAttribute("sandbox", "allow-same-origin,allow-scripts"); 
CxList element = dom.GetMembersOfTarget().FindByShortName("createElement");
CxList iframeString = allLiterals.FindByShortName("iframe", false);
CxList iframeCreate = element.FindByParameters(iframeString);
CxList iframeElement = iframeCreate.GetAssignee();
CxList iframeRef = All.FindAllReferences(iframeElement);
CxList setAttribute = iframeRef.GetMembersOfTarget().FindByShortName("setAttribute");
CxList sandboxAttribute = setAttribute.FindByParameters(allLiterals.FindByShortName("sandbox"));
CxList sandboxTarget = sandboxAttribute.GetTargetOfMembers();
CxList sandboxRef = iframeElement.FindAllReferences(sandboxTarget);
sandboxRef.Add(sandboxTarget.GetAssignee());
result.Add(iframeElement - sandboxRef);

// ReactJS - React.createElement('iframe', {.7..} )
CxList createElem = React_Find_CreateElement();
CxList sandboxProp = React_Find_PropertyKeys().FindByShortName("sandbox").GetByAncs(createElem);
CxList sandboxCreateElem = createElem.FindByParameters(sandboxProp);
CxList createElemWithoutSandbox = createElem - sandboxCreateElem;
result.Add(allLiterals.GetParameters(createElemWithoutSandbox).FindByShortName("iframe"));