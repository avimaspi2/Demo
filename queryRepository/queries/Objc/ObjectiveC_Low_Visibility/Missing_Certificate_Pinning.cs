CxList https = Find_Strings().FindByShortName("*https://*");
CxList methods = Find_Methods();

//init with request get as a parameter a class with delegates for certificate pinning
CxList initWithRequests = methods.FindByShortName("initWithRequest:delegate:");
initWithRequests = initWithRequests.InfluencedBy(https).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

CxList methodDecls = Find_MethodDecls();

foreach (CxList initWithRequest in initWithRequests)
{
	CxList resHeader = All.NewCxList();
	CxList resHeaders = initWithRequest.InfluencedBy(https);
	foreach (CxList tmp in resHeaders.GetCxListByPath())
	{
		resHeader.Add(tmp);
		break;
	}
	
	CxList initWithRequestDelegateParam = All.GetParameters(initWithRequest, 1);
	initWithRequestDelegateParam = initWithRequestDelegateParam.FindByType(typeof(Reference));
	CxList delegateClassDefinitions = All.FindDefinition(initWithRequestDelegateParam);
	
	foreach (CxList delegateClassDefinition in delegateClassDefinitions)
	{
		//delegateClassDefinition must implement canAuthenticateAgainstProtectionSpace
		CxList canAuthenticateAgainstProtectionSpace = methodDecls.FindByShortName("connection:canAuthenticateAgainstProtectionSpace:");
		CxList ancs = canAuthenticateAgainstProtectionSpace.GetByAncs(delegateClassDefinition);
		if (ancs.Count == 0)
		{
			result.Add(resHeader.ConcatenatePath(initWithRequestDelegateParam.ConcatenatePath(delegateClassDefinition)));
			continue;
		}
	
		//delegateClassDefinition must implement didReceiveAuthenticationChallenge
		CxList didReceiveAuthenticationChallenge = methodDecls.FindByShortName("connection:didReceiveAuthenticationChallenge:");
		ancs = didReceiveAuthenticationChallenge.GetByAncs(delegateClassDefinition);
		if (ancs.Count == 0)
		{
			result.Add(resHeader.ConcatenatePath(initWithRequestDelegateParam.ConcatenatePath(delegateClassDefinition)));
			continue;
		}
	
		CxList SecTrustEvaluate = methods.FindByShortName("SecTrustEvaluate");
		ancs = SecTrustEvaluate.GetByAncs(didReceiveAuthenticationChallenge);
		if (ancs.Count == 0)
		{
			result.Add(resHeader.ConcatenatePath(initWithRequestDelegateParam.ConcatenatePath(delegateClassDefinition.ConcatenatePath(didReceiveAuthenticationChallenge))));
			continue;
		}
	}
}