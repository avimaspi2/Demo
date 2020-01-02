// This query returns Backbone Collections

if(cxScan.IsFrameworkActive("Backbone"))
{
	CxList unkRefs = Find_UnknownReference();

	CxList backbone = unkRefs.FindByType("Backbone");

	result = backbone.GetMembersOfTarget().FindByShortName("View").GetMembersOfTarget().FindByShortName("extend").GetAssignee();
}