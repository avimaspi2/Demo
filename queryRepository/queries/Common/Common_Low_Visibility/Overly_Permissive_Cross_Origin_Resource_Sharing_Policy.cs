CxList configVuln = general.Find_CORS_Config();

if(configVuln.Count > 0){
	result = configVuln;
} else {
	/* Find all methods and IndexerRefs to Change Headers
	 * Ex: 
	 * - class.AddHeader("Access-Control-Allow-Origin", "*");
	 * - collection.Headers["Access-Control-Allow-Origin"] = "*";
	**/
	CxList changeHeader = general.Find_Change_Response_Header();
	
	// Separate change header methods from indexerRefs
	CxList methods = changeHeader.FindByType(typeof(MethodInvokeExpr));
	CxList indexerRefs = changeHeader.FindByType(typeof(IndexerRef));
	CxList memberAccesses = changeHeader.FindByType(typeof(MemberAccess));
	
	/* Get the first parameter from methods and the indice from indexerRefs -> headerKey 
	 * to test if they have the value "Access-Control-Allow-Origin" **/
	CxList headerKey = All.GetParameters(methods, 0);
	headerKey.Add(All.GetByAncs(indexerRefs));
	headerKey.Add(memberAccesses);
	
	IAbstractValue absValue = new StringAbstractValue("Access-Control-Allow-Origin"); 
	CxList allowOriginHeader = headerKey.FindByAbstractValue(abstractValue => abstractValue.IncludedIn(absValue));

	/* Inside a While the absint doesn't capture the value, even if the string 
	 * is hardcoded so it's used FindByShortName to capture those cases **/
	CxList allowOriginString = headerKey.FindByShortName("Access-Control-Allow-Origin", false);
	allowOriginHeader.Add(allowOriginString);

	// allowOrigin will contain the vulnerable origins, i.e. "*"
	CxList allowOrigin = All.NewCxList();
	
	/* If we found some form to change header origins (allowOriginHeader or otherOrigins)
	 * we will test if that origin is the vulnerable wildcard  **/
	if (allowOriginHeader.Count > 0) {
		/* Filter the change header methods that have 'Access-Control-Allow-Origin' as 
		 * parameter and get its second parameter **/
		CxList filteredChangeHeader = methods.FindByParameters(allowOriginHeader);
		CxList headerValue = All.GetParameters(filteredChangeHeader, 1);
		
		/* To avoid repetitive results (StringLiteral and Param) **/
		CxList strings = Find_String_Literal();
		headerValue -= strings;
		
		/* Find the relevantIndexerRefs, which have 'Access-Control-Allow-Origin' as index
		 * and add its assigner to headerValue **/
		CxList relevantIndexerRefs = allowOriginHeader.GetByAncs(indexerRefs)
			.GetAncOfType(typeof(IndexerRef));
		headerValue.Add(relevantIndexerRefs.GetAssigner());
		headerValue.Add(memberAccesses.GetAssigner());
		
		// Test if the headerValue have the wildcard value or are influenced by the wildcard
		CxList wildcard = strings.FindByRegex("\"\\*\"");
		CxList allowAllOrigins = headerValue.FindByShortName(wildcard);
		allowAllOrigins.Add(headerValue.InfluencedBy(wildcard));
		allowOrigin.Add(allowAllOrigins);
	
		// "At present, the null origin is significantly more dangerous than the wildcard origin"
		// "...any website can easily obtain the null origin using a sandboxed iframe"
		// source: http://blog.portswigger.net/2016/10/exploiting-cors-misconfigurations-for.html
		CxList anyAbsValueHeader = headerValue.FindByAbstractValue(_ => _ is AnyAbstractValue);
		CxList absintHeaderValue = headerValue - anyAbsValueHeader;
		
		IAbstractValue absValue2 = new StringAbstractValue("null");
		CxList nullOrigin = absintHeaderValue.FindByAbstractValue(abstractValue => absValue2.IncludedIn(abstractValue));
		allowOrigin.Add(nullOrigin);
		
		// Intersection between results found for all origins and null origins are excluded since they are probably wrong.
		allowOrigin -= allowAllOrigins * nullOrigin;
		
		//If headerValue comes from an input or is influenced by it, it is vulnerable
		CxList influencers = general.Find_Inputs();
		influencers.Add(general.Find_DB_Out());
		influencers.Add(general.Find_Read());
		influencers.Add(general.Find_Remote_Requests());
		
		CxList storedOrigin = headerValue.InfluencedBy(influencers);
	
		allowOrigin.Add(storedOrigin.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow));
	}
	/* Get the possible vulnerable elements (other origins) to change headers that don't
	 * follow the standard way common for every language **/
	CxList otherOrigins = general.Find_CORS_Change();
	result = allowOrigin;
	result.Add(otherOrigins);
}