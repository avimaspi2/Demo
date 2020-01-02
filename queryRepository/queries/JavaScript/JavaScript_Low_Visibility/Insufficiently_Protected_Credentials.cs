CxList getJson = Find_Methods();
getJson = getJson.FindByShortName("getJSON");
CxList strings = Find_String_Literal();

foreach (CxList item in getJson)
{	
	CxList firstParam = All.GetParameters(item, 0);
	CxList urls = All.FindAllReferences(firstParam);
	CxList passwordParam = strings.FindByShortName("*password=*", false);
	CxList flow = passwordParam.GetByAncs(urls);
	flow.Add(passwordParam.GetByAncs(urls.GetAncOfType(typeof(AssignExpr))));
	result.Add(getJson.InfluencedBy(flow));
}