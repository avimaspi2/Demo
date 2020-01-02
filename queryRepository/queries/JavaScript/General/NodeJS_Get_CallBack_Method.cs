CxList requestCallback = All.NewCxList();
CxList recCallAsMD = All.NewCxList();

if(param.Length > 0)
{
	try
	{
		requestCallback = param[0] as CxList;
		CxList allMethDecl = Find_MethodDecls();
		CxList lambdas = Find_LambdaExpr();
				
		foreach(CxList requestParam in requestCallback)
		{
			CSharpGraph requestParamGraph = requestParam.TryGetCSharpGraph<CSharpGraph>();
			String requestParamName = requestParamGraph.ShortName;
			
			if(requestParamName.Equals("Lambda")){
				recCallAsMD.Add(lambdas.FindByFathers(requestParam));	
			}
			else{
				recCallAsMD.Add(allMethDecl.FindByShortName(requestParamName));
			}
		}

	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}

result = recCallAsMD;