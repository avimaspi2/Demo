List<KeyValuePair<CxList, CxList>> propsMappingList = querySharedData.GetSharedData("ReactJSPropsMapping")
	as List<KeyValuePair<CxList, CxList>>;
if(propsMappingList != null)
{
	foreach(KeyValuePair<CxList, CxList> pair in propsMappingList)
	{
		CustomFlows.AddFlow(pair.Key, pair.Value);
	}
}