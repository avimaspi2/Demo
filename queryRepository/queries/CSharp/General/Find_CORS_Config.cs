CxList strings = Find_Strings();

CxList stringKeyOrigins = strings.FindByShortName("Access-Control-Allow-Origin");
CxList stringWildCard = strings.FindByRegex("\"\\*\"");

//Find Cors changes in config files:
CxList nameForCORS = stringKeyOrigins.FindByFileName("*.config");
CxList valueStar = stringWildCard.FindByFileName("*.config");

CxList configVuln = nameForCORS * valueStar;

/* Now we want to find configuration of origin header programmatically */
CxList methods = Find_Methods();
CxList commit = methods.FindByMemberAccess("ServerManager.CommitChanges");

CxList configElems = methods.FindByMemberAccess("ConfigurationElementCollection.CreateElement");
configElems.Add(methods.FindByMemberAccess("ConfigurationElement.GetChildElement"));

CxList configParams = All.GetParameters(configElems, 0);

StringAbstractValue addAbsVal = new StringAbstractValue("add");
CxList stringAbsVal = configParams.FindByAbstractValue(_ => _.Contains(addAbsVal));

CxList relevantConfigElems = configElems.FindByParameters(stringAbsVal).GetAssignee();

CxList relevantElem = relevantConfigElems.InfluencedBy(
	All.FindDefinition(commit.GetTargetOfMembers()));

CxList allConfigElems = All.FindAllReferences(relevantElem);
CxList indexRefsConfig = allConfigElems.FindByType(typeof(IndexerRef));

CxList relevantValues = strings;
relevantValues.Add(methods);
relevantValues.Add(All.FindByType(typeof(UnknownReference)));

IAbstractValue absValue = new StringAbstractValue("Access-Control-Allow-Origin"); 
CxList allowOriginHeader = relevantValues.FindByAbstractValue(abstractValue => abstractValue.IncludedIn(absValue)
															&& !(abstractValue is AnyAbstractValue));
allowOriginHeader.Add(stringKeyOrigins);

CxList relevantIndexRefs = allowOriginHeader.GetByAncs(indexRefsConfig).GetAncOfType(typeof(IndexerRef));

CxList origins = relevantIndexRefs.GetAssigner();

IAbstractValue wildcard = new StringAbstractValue("*");
CxList allowAllOrigins = origins.FindByAbstractValue(abstractValue => abstractValue.IncludedIn(wildcard)
													&& !(abstractValue is AnyAbstractValue));
allowAllOrigins.Add(origins.FindByShortName("Access-Control-Allow-Origin"));

result = configVuln;
result.Add(allowAllOrigins);