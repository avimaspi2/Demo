CxList methods = Find_Methods();
CxList memcacheObjects = All.FindByType(typeof(UnknownReference));
memcacheObjects = memcacheObjects.GetByAncs(All.FindByShortName("memcache_connect").GetFathers());
memcacheObjects = memcacheObjects.FindByAssignmentSide(CxList.AssignmentSide.Left);
memcacheObjects = All.FindAllReferences(memcacheObjects);
	
//Build memcache getters
CxList getAttrFirstParam = All.FindByMemberAccess("Memcache.get");
getAttrFirstParam.Add(memcacheObjects.GetMembersOfTarget().FindByShortName("get"));
CxList getAttrSecondParam = methods.FindByShortName("memcache_get");
//Build memcached getters
string[] memcachedGetters = { "get", "getMulti", "getDelayed" }; 
foreach(string getter in memcachedGetters) 
{
	getAttrFirstParam.Add(All.FindByMemberAccess("Memcached." + getter));
	getAttrSecondParam.Add(All.FindByMemberAccess("Memcached." + getter + "ByKey"));
}

result = All.GetParameters(getAttrFirstParam, 0) +
	All.GetParameters(getAttrSecondParam, 1);