CxList methods = Find_Methods();
result = methods.FindByShortName("unset");
CxList Session = All.FindByShortName("_SESSION*").FindByType(typeof(Param));
result = result.FindByParameters(Session);
result.Add(methods.FindByShortName("session_destroy"));
result.Add(methods.FindByShortName("session_unset"));