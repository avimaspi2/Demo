CxList attributes = All.FindByCustomAttribute("EnableCors");
CxList strings = All.FindByType(typeof(StringLiteral));
CxList stringsInAttribute = strings.FindByFathers(attributes);

CxList origins = stringsInAttribute.FindByRegex("origins");

CxList wildcard = stringsInAttribute.FindByRegex("\"\\*\"");
result = origins * wildcard;

CxList httpConfig = All.FindByType("HttpConfiguration");
CxList enableCors = httpConfig.GetMembersOfTarget()
	.FindByShortName("EnableCors");

CxList paramCors = All.GetParameters(enableCors);

CxList corsAttribute = All.FindByType("EnableCorsAttribute");

CxList corsAttributeParam = All.GetParameters(corsAttribute, 0);

IAbstractValue absValue = new StringAbstractValue("origins"); 
CxList corsAttributeOrigins = corsAttributeParam.FindByAbstractValue(abstractValue => abstractValue.IncludedIn(absValue));

CxList relevantCors = corsAttribute.FindByParameters(corsAttributeOrigins);

CxList relevantParam = All.GetParameters(relevantCors, 1).InfluencingOn(paramCors);

result.Add(relevantParam);