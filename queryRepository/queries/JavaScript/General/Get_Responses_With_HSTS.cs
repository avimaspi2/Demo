if(param.Length == 2){
	CxList responses = (CxList) param[0];
	CxList headers = (CxList) param[1];
	CxList resps = Find_HTTP_Responses();
	
	result.Add(responses.InfluencedBy(headers));
	
	headers = headers - result.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
	foreach(CxList header in headers){
		CxList headerDecl = header.GetAncOfType(typeof(MethodDecl));
		if (headerDecl.Count > 0) {
			int fileid = headerDecl.GetFirstGraph().LinePragma.GetFileId();
			CxList unknownRef = Find_UnknownReference().FindByShortName(headerDecl.GetName() + "var");
			unknownRef = unknownRef.FindByFileId(fileid);
			CxList headerChangeMethod = Find_Change_Response_Header().FindByParameters(unknownRef);
			unknownRef = unknownRef.GetByAncs(headerChangeMethod);
			
			CxList response = headerChangeMethod.GetTargetOfMembers() * resps;
			
			CxList flow = headerChangeMethod.InfluencedBy(header);
			
			flow = flow.ConcatenatePath(response, false);
			
			flow = flow.ConcatenatePath(All.FindAllReferences(response) * responses, false);
			result.Add(flow);
		}
	}
}