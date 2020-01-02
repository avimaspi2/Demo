//find all indexer access using counf or length
CxList members = All.FindByType(typeof(MemberAccess));
CxList counts = members.FindByShortNames(new List <string> {"Count",  "Length"});

CxList listsUsingCount = counts.GetFathers().FindByType(typeof(IndexerRef));
counts = counts.GetByAncs(listsUsingCount);

foreach (CxList count in counts)
{
	CxList listUsingCount = count.GetFathers();
	listUsingCount = listUsingCount.FindByType(typeof(IndexerRef));
	CxList usedCount = count.GetTargetOfMembers();
	result.Add(listUsingCount.FindByShortName(usedCount));
}