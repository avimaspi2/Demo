CxList session = All.FindByShortName(@"_SESSION_*");
CxList leftSideSession = session.FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList inputs = Find_Inputs() - leftSideSession;
CxList sanitize = Find_General_Sanitize();

CxList methods = Find_Methods();
CxList memcache = methods.FindByShortName("memcache_set") + All.FindByMemberAccess("Memcache.set");

result = (leftSideSession + memcache).InfluencedByAndNotSanitized(inputs, sanitize);