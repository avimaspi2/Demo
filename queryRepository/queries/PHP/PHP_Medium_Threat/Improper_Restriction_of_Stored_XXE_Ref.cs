/// <summary>
/// This query searches for XML files created from a DB read
/// Sources	-> DB reads and DB outputs
/// Sincs	-> XML files
/// </summary>

CxList inputs = Find_Read();
inputs.Add(Find_DB_Out());

CxList xxe_results = Find_XXE_SimpleXml();
xxe_results.Add(Find_XXE_XmlDOM());
xxe_results.Add(Find_XXE_XMLReader());

CxList xxe_results_vulnerable = xxe_results - Find_XXE_Sanitizers();

CxList tempResult = xxe_results_vulnerable.DataInfluencedBy(inputs);

//Extreme case where the source is also an XML file
foreach(CxList res in tempResult.GetCxListByPath()){
	CxList start = res.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
	CxList end = res.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
	
	if(start.GetParameters(end).Count == 0) {
		result.Add(res);
	}
}