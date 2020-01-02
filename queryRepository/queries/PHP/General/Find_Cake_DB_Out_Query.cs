// Collects all the SQL Query methods applied to a CakePHP Model
CxList thisRef = All.FindByType(typeof(ThisRef));
CxList classes = All.FindByType(typeof(ClassDecl));
CxList thisRefInappmodel = thisRef.GetByAncs(classes.InheritsFrom("AppModel"));
CxList candidates = thisRefInappmodel.GetMembersOfTarget();
candidates.Add(candidates.GetMembersOfTarget());
result = candidates.FindByShortName("query");