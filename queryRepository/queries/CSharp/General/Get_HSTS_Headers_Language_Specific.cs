if(param.Length == 1){
	CxList nodes = (CxList) param[0];
	/*handle IndexerRef assigns*/
	result.Add(All.FindByShortName("Strict-Transport-Security", false)
		.GetByAncs(nodes.FindByType(typeof(IndexerRef)))
		.GetAncOfType(typeof(IndexerRef)).GetAssigner());
}