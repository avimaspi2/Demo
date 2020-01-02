string JQueryObjName = "jquery";
CxList FindMethods = Find_Methods();

// Find all instances of JQuery objects use.
CxList JQueryObj = All.FindByShortNames(new List<string>{JQueryObjName,"$"}, false);

CxList Methods = FindMethods.FindByMemberAccess("$.*");
Methods.Add(FindMethods.FindByShortName("$"));
Methods.Add(FindMethods.FindByShortName(JQueryObjName, false).GetMembersOfTarget());

CxList influencedMethods = FindMethods.InfluencedBy(JQueryObj);
Methods.Add(influencedMethods.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly));

CxList AddMethods = JQueryObj;

int MAXDEPTH = 5;
int i = MAXDEPTH;
while (AddMethods.Count > 0 && i >= 0)
{
	AddMethods = AddMethods.GetAncOfType(typeof(MethodInvokeExpr)) - JQueryObj;
	Methods.Add(AddMethods);
	i--;
}

//support for jQuery.method / jQuery.xxx.method /jQuery.xxx.xxx.method
CxList members = JQueryObj.GetMembersOfTarget();

i = MAXDEPTH;

while (i > 0 && members.Count > 0)
{
	AddMethods = members.FindByType(typeof(MethodInvokeExpr));
	Methods.Add(AddMethods);
	members = members.GetMembersOfTarget();
	i--;
}
i = MAXDEPTH;

while (i >= 0)
{
	// Add indexerRef members
	Methods.Add(Methods.GetFathers().FindByType(typeof(IndexerRef)).GetMembersOfTarget());
	Methods.Add(Methods.GetMembersOfTarget());	
	i--;
}

result = Methods;
// add all methods that starts from $(this)
CxList thisRef = Find_Reference().FindByType(typeof(ThisRef));
CxList JQueryDomObj = Methods.FindByParameters(thisRef);
CxList addResults = FindMethods.DataInfluencedBy(JQueryDomObj).GetPathsOrigins();

result.Add(addResults);