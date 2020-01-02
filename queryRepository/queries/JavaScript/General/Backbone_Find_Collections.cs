// This query returns Backbone Collections 

if(cxScan.IsFrameworkActive("Backbone"))
{
	CxList unkRefs = Find_UnknownReference();

	CxList backbone = unkRefs.FindByType("Backbone");

	result = backbone.GetMembersOfTarget().FindByShortName("Collection").GetMembersOfTarget().FindByShortName("extend").GetAssignee();
}