CxList codeConfig = Find_HSTS_Configuration_In_Code();
if(codeConfig.Count > 0){
	CxList headerChanges = codeConfig.FindByType(typeof(MethodInvokeExpr));
	CxList headers = Get_HSTS_Headers(headerChanges);
	CxList httpResponses = Find_HTTP_Responses();
	CxList listenAndServe = Find_Methods().FindByShortName("ListenAndServe");
	foreach(CxList header in headers){
		if(Validate_HSTS_Header(header).Count>0){
			CxList flow = header;
			CxList headerSet = header.GetAncOfType(typeof(MethodInvokeExpr));
			flow = flow.ConcatenatePath(headerSet);
			flow = flow.ConcatenatePath(httpResponses.GetTargetsWithMembers(headerSet,2));
			CxList middlewareDecl = header.GetAncOfType(typeof(MethodDecl));
			CxList allRefsOfMiddleware = All.FindAllReferences(middlewareDecl);
			flow = flow.ConcatenatePath(middlewareDecl);
			flow = flow.ConcatenatePath(allRefsOfMiddleware.GetParameters(listenAndServe));
			flow = flow.ConcatenatePath(listenAndServe.FindByParameters(allRefsOfMiddleware));
			result.Add(flow);
		}
	}
	
}