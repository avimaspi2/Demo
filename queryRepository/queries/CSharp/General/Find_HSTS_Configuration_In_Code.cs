/* This query finds configuration of HSTS headers programmatically */
CxList commit = All.FindByMemberAccess("ServerManager.CommitChanges");
CxList configElems = All.FindByMemberAccess("ConfigurationElement.GetChildElement");
CxList configParams = All.GetParameters(configElems, 0);

CxList stringAbsVal = All.FindByAbstractValue(_ => _ is StringAbstractValue);
StringAbstractValue hstsAbsVal = new StringAbstractValue("hsts");
stringAbsVal = All.FindByAbstractValue(_ => _.Contains(hstsAbsVal));

CxList relevantParams = configParams * stringAbsVal;

CxList relevantConfigElems = configElems.FindByParameters(relevantParams).GetAssignee();

CxList flow = relevantConfigElems.InfluencedBy(
	All.FindDefinition(commit.GetTargetOfMembers()));
result = flow * relevantConfigElems;