if(param.Length == 2){
	CxList responses = Find_HTTP_Responses();
	CxList leftTargetOfResponseWrite = (CxList) param[0];
	CxList hstsHeaders = (CxList) param[1];
	
	foreach(CxList header in hstsHeaders){
		CxList setHeaderFunc = header.GetAncOfType(typeof(MethodInvokeExpr));
		CxList flow = header.ConcatenatePath(setHeaderFunc , false);
		CxList maybeResponse = setHeaderFunc.GetLeftmostTarget() * responses;
		if(maybeResponse.Count == 0){
			maybeResponse = maybeResponse.GetMembersOfTarget() * responses;
		}
		
		flow = flow.ConcatenatePath(maybeResponse,false);
		
		CxList aux = leftTargetOfResponseWrite.GetMembersOfTarget().InfluencedBy(maybeResponse);
		
		flow = flow.ConcatenatePath(aux.GetLeftmostTarget());
		
		result.Add(flow);	
	}
}