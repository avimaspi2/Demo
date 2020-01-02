// General query to find Vue Router API inputs
CxList unknownRefs = Find_UnknownReference();
CxList objCreations = Find_ObjectCreations();

//Get Vue instances references
CxList vueInstances = Find_ViewModelComponent();
CxList vueInstancesRef = unknownRefs.FindAllReferences(vueInstances);
vueInstancesRef.Add(Find_ThisRef());

// Get Route instances
CxList routesRefs = unknownRefs.FindByShortName("$route");
CxList memberAccessRoutes = Find_MemberAccesses().FindByShortName("$route");
routesRefs.Add(vueInstancesRef.GetMembersOfTarget());
routesRefs.Add(memberAccessRoutes);

//Get VueRouter instances
CxList vueRouterObj = objCreations.FindByShortName("VueRouter").GetAssignee();
routesRefs.Add(unknownRefs.FindAllReferences(vueRouterObj));

// List of route fields
List<string> routeFields = new List<string>{"params","path","query","hash","fullPath"};

result = routesRefs.GetMembersOfTarget().FindByShortNames(routeFields);