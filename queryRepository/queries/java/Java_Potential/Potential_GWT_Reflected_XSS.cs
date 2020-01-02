CxList methodDecl = Find_MethodDecls();
CxList public_methods = methodDecl.FindByFieldAttributes(Modifiers.Public);
CxList remoteService = All.InheritsFrom("RemoteServiceServlet");
public_methods = public_methods.GetByAncs(remoteService);
CxList XSS_GWT_Output = GWT_XSS_Outputs();
CxList Input = Find_Potential_Inputs();
CxList onSuccess = methodDecl.FindByShortName("onSuccess");

foreach(CxList curMethods in public_methods)
{
	CxList returnStmt = All.GetByAncs(curMethods).FindByType(typeof(ReturnStmt));
	returnStmt = All.GetByAncs(returnStmt).DataInfluencedBy(Input);
	if(returnStmt.Count > 0)//there is an input influencing on return of rmi call
	{
		CSharpGraph graph = curMethods.TryGetCSharpGraph<CSharpGraph>();
		string name = graph.ShortName;
		CxList clientRMIInvoke = Find_Methods().FindByShortName(name);
		CxList clientAsync = All.GetParameters(clientRMIInvoke);
		
		clientAsync.Add(All.DataInfluencingOn(All.FindAllReferences(clientAsync)));
		result.Add(clientAsync);
	}
}

result = All.FindAllReferences(All.GetByAncs(result));
result = XSS_GWT_Output.DataInfluencedBy(All.GetParameters(result * onSuccess));