CxList methods = Find_Methods();

CxList queryMethods = methods.FindByShortName("Query");

result.Add(methods.FindByShortName("Iter").InfluencedBy(queryMethods).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly));
result.Add(methods.FindByShortName("Scan").InfluencedByAndNotSanitized(queryMethods, methods.FindByShortName("Iter")).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly));

CxList listImports = All.FindByMemberAccess("\"github.com/gocql/gocql\".NewCluster");
CxList listAssign = listImports.GetAssignee();
CxList listRef = All.FindAllReferences(listAssign);
CxList listRefTarget = listRef.GetMembersOfTarget();
CxList listRefCreateSession = listRefTarget.FindByShortName("CreateSession");
CxList listSession = listRefTarget.GetFathers().GetAssignee();

CxList listRefExecuteBatch = All.FindAllReferences(listSession).GetMembersOfTarget().FindByShortName("ExecuteBatch");
result.Add(listRefExecuteBatch);