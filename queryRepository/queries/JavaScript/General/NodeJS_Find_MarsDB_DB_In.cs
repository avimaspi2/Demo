CxList methods = Find_Methods();
CxList require = methods.FindByShortName("require");
CxList parameters = Find_Param().FindByShortName("\"marsdb\"");
CxList marsdbRequire = parameters.GetParameters(require);

// If Marsdb is used
if (marsdbRequire.Count > 0) 
{
	List <string> marsDbMethods = new List<string> {"update", "find", "findOne", "insert", "insertAll", "remove"};
	
	CxList assignExpr = Find_AssignExpr();
	CxList unkRefs = Find_UnknownReference();
	CxList objCreation = Find_ObjectCreations();
	CxList mardbCollections = objCreation.FindByShortName("Collection").GetAssignee();
		
	List<string> collectionNames = new List<string>();	
	foreach (CxList collection in mardbCollections)
	{
		collectionNames.Add(collection.GetName());
	}
		
	CxList imported = require.GetAssignee();
	CxList allRefsFromImported = unkRefs.FindAllReferences(imported);
	
	CxList exports = unkRefs.GetByAncs(assignExpr).FindByShortName("cxExports");
	CxList exported = exports.GetAssigner();
	CxList allInfluencedByExported = unkRefs.DataInfluencedBy(exported).GetStartAndEndNodes(CxList.GetStartEndNodesType.AllNodes);
	
	CxList importedDbs = allRefsFromImported * allInfluencedByExported;
	CxList importedDbsCollections = unkRefs.FindAllReferences(importedDbs);
	importedDbsCollections.Add(importedDbsCollections.GetMembersOfTarget());
	importedDbsCollections = importedDbsCollections.FindByShortNames(collectionNames);
	
	CxList importedDbsMethods = importedDbsCollections.GetMembersOfTarget().FindByShortNames(marsDbMethods);
	result = importedDbsMethods;
}