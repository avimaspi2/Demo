CxList containerResponseContext = All.FindByMemberAccess("ContainerResponseContext.getHeaders");
CxList headers = All.FindAllReferences(containerResponseContext.GetAssignee());

headers = headers.GetMembersOfTarget();

CxList headersMethods = headers.FindByShortNames(
	new List<string>(){"putAll","replaceAll"});
CxList vulnHeaders = All.NewCxList();

if (headersMethods.Count > 0){
	
	CxList strings = Find_Strings();
	CxList wildcard = strings.FindByRegex("\"\\*\"");
	CxList influencers = Find_Inputs();
	influencers.Add(Find_DB_Out());
	influencers.Add(Find_Read());
	influencers.Add(Find_Remote_Requests());

	vulnHeaders = headersMethods.InfluencedBy(wildcard);
	vulnHeaders.Add(headersMethods.InfluencedBy(influencers));
}

CxList corsRegistryMapping = All.FindByMemberAccess("CorsRegistry.addMapping");
CxList allowedOrigins = corsRegistryMapping.GetMembersOfTarget().FindByShortName("allowedOrigins");
CxList parameters = All.GetParameters(allowedOrigins);
vulnHeaders.Add(parameters.FindByAbstractValue(p => 
	(p is StringAbstractValue) && ((p as StringAbstractValue).Content == "*")));

result = vulnHeaders;