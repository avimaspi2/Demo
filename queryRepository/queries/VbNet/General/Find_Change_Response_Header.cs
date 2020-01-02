CxList responses = Find_HTTP_Responses();

CxList responseMembers = responses.GetMembersOfTarget();

CxList responseMethods = responseMembers.FindByShortNames(
	new List<string>(){"AddHeader","AppendHeader"},false);

CxList headerCollections = responseMembers.FindByShortName("Headers",false);

headerCollections.Add(All.FindByTypes(new string[]
	{"WebHeaderCollection","HttpResponseHeaders","NameValueCollection"}, false));

headerCollections = All.FindAllReferences(headerCollections);

CxList headerMethods = headerCollections.GetMembersOfTarget().FindByShortName("Add",false);

CxList headerIndexerRefs = headerCollections.GetAncOfType(typeof(IndexerRef));

result = responseMethods;
result.Add(headerMethods);
result.Add(headerIndexerRefs);