if(param.Length > 0)
{
	List<string> keyShortNameList = param[0] as List<string>;
	List<KeyValuePair<CxList, CxList>> propsMappingList = querySharedData.GetSharedData("ReactJSPropsMapping")
		as List<KeyValuePair<CxList, CxList>>;
	if(propsMappingList != null) 
	{
		foreach(KeyValuePair<CxList, CxList> pair in propsMappingList)
		{
			result.Add(pair.Key.FindByShortNames(keyShortNameList));
		}
	}
	result -= Find_Parameters();
	result -= Find_MemberAccesses();
	result -= Find_Methods();
}