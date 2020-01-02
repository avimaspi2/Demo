if (All.isWebApplication)
{
	CxList aspSanitized = Find_XSRF_Sanitize();
	
	CxList preloads = aspSanitized.FindByShortName("*PreLoad", false);
	if(preloads.Count > 0){
		aspSanitized.Add(All.GetClass(preloads));
	}
		
	CxList sanitizers = Find_Boolean_Operators();
	
	CxList requests = Find_Interactive_Inputs();
	requests.Add(All.FindByName("*Request.Querystring*", false));
	requests -= requests.GetByAncs(aspSanitized);
	
	CxList db = Find_DB_In();
	CxList strings = Find_Strings();
	

	CxList write = strings.FindByName("*update*", StringComparison.OrdinalIgnoreCase);
	write.Add(strings.FindByName("*delete*", StringComparison.OrdinalIgnoreCase));
	write.Add(strings.FindByName("*insert*", StringComparison.OrdinalIgnoreCase));

	
	result = db.InfluencedByAndNotSanitized(write, sanitizers).InfluencedByAndNotSanitized(requests, sanitizers);
	
	result.Add(Find_ASP_MVC_XSRF());
}