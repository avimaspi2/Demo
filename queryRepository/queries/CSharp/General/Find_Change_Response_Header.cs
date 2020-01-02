CxList responses = Find_Response();

CxList responseMembers = responses.GetMembersOfTarget();
CxList responseMethods = responseMembers.FindByShortNames(
	new List<string>(){"AddHeader","AppendHeader"});

CxList headerCollections = responseMembers.FindByShortName("Headers");
headerCollections.Add(All.FindByTypes(new string[]
	{"WebHeaderCollection","HttpResponseHeaders","NameValueCollection"}) * 
	(responses + responses.GetRightmostMember()).GetAssigner());

headerCollections = All.FindAllReferences(headerCollections);

CxList headerMethods = headerCollections.GetMembersOfTarget().FindByShortName("Add");

CxList headerIndexerRefs = headerCollections.GetAncOfType(typeof(IndexerRef));

result = responseMethods;
result.Add(headerMethods);
result.Add(headerIndexerRefs);