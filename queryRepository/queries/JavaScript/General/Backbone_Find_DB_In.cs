if(cxScan.IsFrameworkActive("Backbone"))
{
	List<string> modelMethodNames = new List<string>{"save", "destroy", "sync"};
	List<string> collectionMethodNames = new List<string>{"create", "sync"};

	CxList unkRefs = Find_UnknownReference();
	CxList parameters = Find_Param();

	// Get Backbone collections method parameters
	CxList backboneCollections = Backbone_Find_Collections();
	CxList backboneCollectionsMethods = unkRefs.FindByType(backboneCollections).GetMembersOfTarget().FindByShortNames(collectionMethodNames);
	result = parameters.GetParameters(backboneCollectionsMethods);

	// Get Backbone models methods paremeters
	CxList backboneViews = Backbone_Find_Views();
	CxList viewModels = Find_MemberAccesses().FindByShortName("model").GetByAncs(backboneViews.GetAncOfType(typeof(AssignExpr)));

	// View models being sent to the persistency layer and the parameters of their methods
	CxList viewModelsBeingPersisted = viewModels.GetMembersOfTarget().FindByShortNames(modelMethodNames);
	result.Add(parameters.GetParameters(viewModelsBeingPersisted));
	result.Add(viewModelsBeingPersisted.GetTargetOfMembers());
}