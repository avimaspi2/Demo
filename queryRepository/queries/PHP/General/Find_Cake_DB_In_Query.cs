CxList thisRef = All.FindByType(typeof(ThisRef));
CxList thisRefInappmodel = thisRef.GetByAncs(All.FindByType(typeof(ClassDecl)).InheritsFrom("AppModel"));
CxList query = All.FindByShortName("query");
CxList candidates = thisRefInappmodel.GetMembersOfTarget();
candidates.Add(candidates.GetMembersOfTarget());
result.Add(All.GetParameters(candidates * query));