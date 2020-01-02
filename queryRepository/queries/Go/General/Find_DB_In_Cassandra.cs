CxList methods = Find_Methods();

CxList queryMethods = methods.FindByShortName("Query");

result.Add(methods.FindByShortName("Exec").InfluencedBy(queryMethods).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly));

CxList listImports = All.FindByMemberAccess("\"github.com/gocql/gocql\".NewCluster");
CxList listAssign = listImports.GetAssignee();
CxList listRef = All.FindAllReferences(listAssign);

CxList listRefTargets = listRef.GetMembersOfTarget();

CxList listRefCreateSession = listRefTargets.FindByShortName("CreateSession");
CxList listSession = listRefTargets.GetFathers().GetAssignee();

CxList listRefExecuteBatch = All.FindAllReferences(listSession).GetMembersOfTarget().FindByShortName("ExecuteBatch");
result.Add(listRefExecuteBatch);