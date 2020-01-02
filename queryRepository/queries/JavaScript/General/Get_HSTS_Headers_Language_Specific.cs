if(param.Length == 1){
	CxList headerChangingMethods = (CxList) param[0];
	/*handle methods with JSON object as parameter*/
	CxList fieldDecls = Find_FieldDecls().FindByShortName("Strict-Transport-Security");
	
	CxList headersInJSON = All.FindAllReferences(fieldDecls).GetAssigner().FindByType(typeof(StringLiteral));
	
	result.Add(headersInJSON.InfluencingOn(headerChangingMethods)
		.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly));
}