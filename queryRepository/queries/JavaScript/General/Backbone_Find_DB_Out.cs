if(cxScan.IsFrameworkActive("Backbone"))
{
	List<string> modelMethodNames = new List<string>{"save", "destroy", "fetch", "sync"};
	List<string> collectionMethodNames = new List<string>{"create", "fetch", "sync"};

	CxList unkRefs = Find_UnknownReference();
	CxList lambdaExpr = Find_LambdaExpr();
	CxList paramDecls = Find_ParamDecl();

	// Get Backbone collections
	CxList backboneCollections = Backbone_Find_Collections();
	CxList backboneCollectionMethods = unkRefs.FindByType(backboneCollections).GetMembersOfTarget().FindByShortNames(collectionMethodNames);
	result = paramDecls.GetParameters(lambdaExpr.GetByAncs(backboneCollectionMethods));
	result.Add(backboneCollectionMethods.FindByShortName("fetch").GetTargetOfMembers());

	// Get Backbone views
	CxList backboneViews = Backbone_Find_Views();
	CxList viewModels = Find_MemberAccesses().FindByShortName("model").GetByAncs(backboneViews.GetAncOfType(typeof(AssignExpr)));
	CxList viewModelsMethods = viewModels.GetMembersOfTarget().FindByShortNames(modelMethodNames);
	result.Add(paramDecls.GetParameters(lambdaExpr.GetByAncs(viewModelsMethods)));
}