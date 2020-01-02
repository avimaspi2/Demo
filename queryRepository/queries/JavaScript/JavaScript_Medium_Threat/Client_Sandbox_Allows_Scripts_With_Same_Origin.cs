//use of iframe sandboxing
CxList iframe = All.FindByShortName("iframe__*");

CxList allStrings = Find_String_Literal();
CxList sandbox = All.FindByShortName("sandbox");

foreach(CxList attribute in iframe)
{
	CxList parameter = allStrings.GetParameters(sandbox.GetByAncs(attribute));

	if(parameter.FindByShortName("*allow-scripts*").Count > 0 &&
		parameter.FindByShortName("*allow-same-origin*").Count > 0)
	{
		result.Add(parameter);
	}
}

// element.innerHTML = "<iframe sandbox=allow-same-origin,allow-scripts ...
CxList dom = Find_DOM_Elements();
CxList innerHTMLDyn = dom.GetMembersOfTarget().FindByShortName("innerHTML");
CxList innerHTMLDynValue = innerHTMLDyn.GetAssigner();

CxList innerHtmlAssignedToString =  innerHTMLDynValue
	.FindByAbstractValue(_ => _ is StringAbstractValue val && 
	val.Content.Contains("iframe") &&
	val.Content.Contains("allow-scripts") &&
    val.Content.Contains("allow-same-origin"));

result.Add(innerHtmlAssignedToString);


// var element = document.createElement("iframe");
// elem.setAttribute("sandbox", "allow-same-origin,allow-scripts"); 
CxList element = dom.GetMembersOfTarget().FindByShortName("createElement");
CxList iframeString = allStrings.FindByShortName("iframe");
CxList iframeCreate = element.FindByParameters(iframeString);
iframeCreate.Add(All.FindAllReferences(iframeCreate.GetAssignee()));
CxList setAttribute = iframeCreate.GetMembersOfTarget().FindByShortName("setAttribute");
CxList sandboxAttribute = setAttribute.FindByParameters(allStrings.FindByShortName("sandbox"));

foreach(CxList attribute in sandboxAttribute)
{
	
	CxList parameter = allStrings.GetParameters(attribute, 1);

	if(parameter.FindByShortName("*allow-scripts*").Count > 0 &&
		parameter.FindByShortName("*allow-same-origin*").Count > 0)
	{
		result.Add(attribute);
	}
}

//React
result.Add(React_Client_Sandbox_Allows_Scripts_With_Same_Origin());