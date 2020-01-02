CxList responses = Find_HTTP_Responses();
CxList responsesInReturnStmt = responses.GetByAncs(responses.GetAncOfType(typeof(ReturnStmt)));
CxList ur = Find_UnknownReference();
foreach(CxList ret in responsesInReturnStmt){
	CxList influencedByResponseInReturnStmt = All.InfluencedBy(ret);
	influencedByResponseInReturnStmt = influencedByResponseInReturnStmt
		.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
	CxList route = ur.FindAllReferences(influencedByResponseInReturnStmt);
	if( route.GetAncOfType(typeof(MethodDecl))
	.Equals( ret.GetAncOfType(typeof(MethodDecl)) )){
		result.Add(influencedByResponseInReturnStmt);
	}
}