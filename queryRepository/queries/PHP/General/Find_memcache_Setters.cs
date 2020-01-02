//Build memcache and memcached setters
CxList methods = Find_Methods();
CxList memcacheObjects = All.FindByType(typeof(UnknownReference));
memcacheObjects = memcacheObjects.GetByAncs(All.FindByShortName("memcache_connect").GetFathers());
memcacheObjects = memcacheObjects.FindByAssignmentSide(CxList.AssignmentSide.Left);
memcacheObjects = All.FindAllReferences(memcacheObjects).GetMembersOfTarget();

string[] memcacheSetters = {"set", "add", "increment", "decrement", "replace", "append",
	"prepend", "setMulti" };
CxList setAttrFirstParam = All.NewCxList();
CxList setAttrSecondParam = All.FindByMemberAccess("Memcache.cas");
foreach(string setter in memcacheSetters) 
{
	setAttrFirstParam.Add(All.FindByMemberAccess("Memcache." + setter));
	setAttrFirstParam.Add(All.FindByMemberAccess("Memcached." + setter));
	setAttrFirstParam.Add(memcacheObjects.FindByShortName(setter));
	
	setAttrSecondParam.Add(All.FindByMemberAccess("Memcached." + setter + "ByKey"));
	setAttrSecondParam.Add(methods.FindByShortName("memcache_" + setter));
}
result = All.GetParameters(setAttrFirstParam, 0) +
	All.GetParameters(setAttrSecondParam, 1);