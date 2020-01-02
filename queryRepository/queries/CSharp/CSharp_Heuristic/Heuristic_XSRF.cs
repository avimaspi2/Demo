if (All.isWebApplication)
{	
	CxList possible_db = Find_DB_Heuristic();

	// Exclude cache related methods
	possible_db -= possible_db.FindByName("System.Web.HttpRuntime.Cache.*");
	possible_db -= possible_db.FindByName("*Cache.GetCacheItem");
	
	possible_db -= possible_db.DataInfluencedBy(possible_db);
	
	if (possible_db.Count > 0)
	{
		CxList requests = Find_Interactive_Inputs() + All.FindByName("*Request.QueryString*");
		CxList strings = Find_Strings();
		CxList write = strings.FindByName("*update*", StringComparison.OrdinalIgnoreCase) +
			strings.FindByName("*delete*", StringComparison.OrdinalIgnoreCase) +
			strings.FindByName("*insert*", StringComparison.OrdinalIgnoreCase);

		result = possible_db.DataInfluencedBy(write).DataInfluencedBy(requests);
	}
}