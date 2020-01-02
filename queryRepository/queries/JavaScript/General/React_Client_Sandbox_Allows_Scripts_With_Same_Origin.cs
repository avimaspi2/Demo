CxList allStrings = Find_String_Literal();
CxList createElem = React_Find_CreateElement();
CxList iframeComponent = allStrings.GetParameters(createElem).FindByShortName("iframe");
CxList iframeCreateElem = createElem.FindByParameters(iframeComponent);
CxList sandboxProp = React_Find_PropertyKeys().FindByShortName("sandbox").GetByAncs(iframeCreateElem);
CxList sandboxValues = sandboxProp.GetAssigner();

foreach(CxList values in sandboxValues)
{
	if(values.FindByShortName("*allow-scripts*").Count > 0 &&
		values.FindByShortName("*allow-same-origin*").Count > 0)
	{
		result.Add(values);
	}
}