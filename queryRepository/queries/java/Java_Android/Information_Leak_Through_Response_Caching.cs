//SAP Improvments new Query for Response cache 
//Check if cache response is installed
CxList responseCacheInstalled = All.FindByMemberAccess("HttpResponseCache.install");
responseCacheInstalled.Add(All.FindByShortName("android.net.http.HttpResponseCache").GetAncOfType(typeof(MethodInvokeExpr)));

if(responseCacheInstalled != null)
{
	//Should be installed on the page load event onCreate
	if(responseCacheInstalled.GetAncOfType(typeof(MethodDecl)).FindByShortName("onCreate") != null)
	{
		//Preparing source and sink
		CxList HttpRequest = All.FindByMemberAccess("URL.openConnection");
		CxList HttpResponse = All.FindByMemberAccess("HttpURLConnection.getInputStream");
		//Adding sanitizers
		CxList IgnorCacheSanitizer = All.NewCxList();
		CxList IgnorCacheSet = All.FindByMemberAccess("HttpURLConnection.setUseCaches");
		CxList IgnorCacheHeader = All.FindByMemberAccess("HttpURLConnection.setRequestProperty");
		
		CxList IgnorCacheFalseParameter = All.GetParameters(IgnorCacheSet, 0).FindByType(typeof(Param)).FindByShortName("false");
		CxList IgnorCacheHeaderParam1 = All.GetParameters(IgnorCacheHeader, 0).FindByType(typeof(Param)).FindByShortName("\"\"Cache-Control\"\"");
		CxList IgnorCacheHeaderParam2 = All.GetParameters(IgnorCacheHeader, 1).FindByType(typeof(Param)).FindByShortName("\"\"no-cache\"\"");
		CxList IgnorCacheHeaderParam3 = All.GetParameters(IgnorCacheHeader, 1).FindByType(typeof(Param)).FindByShortName("\"\"max-age=0\"\"");
		
		if(IgnorCacheFalseParameter.Count > 0)
		{
			IgnorCacheSanitizer.Add(IgnorCacheSet.GetTargetOfMembers());	
		}
		
		if (IgnorCacheHeaderParam1.Count > 0)
		{
			if(IgnorCacheHeaderParam2.Count > 0)
			{
				IgnorCacheSanitizer.Add(IgnorCacheHeader.GetTargetOfMembers());
			}
			
			if(IgnorCacheHeaderParam3.Count > 0)
			{
				IgnorCacheSanitizer.Add(IgnorCacheHeader.GetTargetOfMembers());
			}
		}
		result = HttpResponse.InfluencedByAndNotSanitized(HttpRequest, IgnorCacheSanitizer);
	}
}