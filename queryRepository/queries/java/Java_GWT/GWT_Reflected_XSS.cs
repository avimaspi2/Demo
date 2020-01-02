CxList public_methods = Find_GWT_Server_Input_Methods();
if (public_methods.Count > 0)
{
	CxList methodDecl = Find_MethodDeclaration();
	CxList XSS_GWT_Output = GWT_XSS_Outputs();
	CxList Input = Find_Inputs();
	CxList findMethods = Find_Methods();
	CxList onSuccess = methodDecl.FindByShortName("onSuccess");
	//CxList onFailure = methodDecl.FindByShortName("onFailure");

	if(onSuccess != null && onSuccess.Count > 0)
	{
		CxList returnStmtAll = Find_ReturnStmt().GetByAncs(public_methods); 
		CxList ancsOfReturn = All.GetByAncs(returnStmtAll);
		CxList inReturnIBInput = ancsOfReturn.DataInfluencedBy(Input);
		CxList parameters = All.GetParameters(findMethods);
		CxList paramsRef = All.FindAllReferences(parameters);
		CxList allInfluencingOnParamsRef = All.DataInfluencingOn(paramsRef).GetPathsOrigins();

		foreach(CxList curMethods in public_methods)
		{
			CxList curReturn = returnStmtAll.GetByAncs(curMethods);
			CxList returnStmt = inReturnIBInput.GetByAncs(curReturn);

			if(returnStmt.Count > 0)//there is an input influencing on return of rmi call
			{
				CSharpGraph graph = curMethods.TryGetCSharpGraph<CSharpGraph>();
				if (graph == null || graph.ShortName == null)
					continue;
				string name = graph.ShortName;
				CxList clientRMIInvoke = findMethods.FindByShortName(name);
				CxList clientAsync = parameters.GetParameters(clientRMIInvoke);
			
				CxList additionalData = allInfluencingOnParamsRef.DataInfluencingOn(paramsRef.FindAllReferences(clientAsync)).GetPathsOrigins();
			
				clientAsync.Add(additionalData);
				result.Add(clientAsync);
			}
		}
		result = All.FindAllReferences(All.GetByAncs(result));
		result = XSS_GWT_Output.DataInfluencedBy(All.GetParameters(result * onSuccess));
	}
}