//Session Fixation vulnerability consists of two parts
//One of them is an user being able to influence the sessionID
//The second one is once a login happens we need to make sure that the cookie is refreshed properly so that an attacker cannot use
//a cookie from a previous session to login
//This query as it is only covers the first part

CxList sessionAssigned = Find_Session_Create();

CxList methods = sessionAssigned.GetMembersOfTarget();
CxList indexerRefs = sessionAssigned.FindByType(typeof(IndexerRef));
CxList sessionIdIndexerRefs = indexerRefs.FindByShortName("*SessionID");
CxList sessionIdMethods = methods.FindByShortName("SessionID");

CxList inputs = Find_Inputs();

CxList sessionIds = sessionIdMethods + sessionIdIndexerRefs.GetFathers();
CxList sessionIdsInfluenced = sessionIds.InfluencedBy(inputs);

result = sessionIdsInfluenced;