CxList allDataInfluencedByConn = Find_DB_Conn_SQLAlchemy();

CxList methods = All.FindAllReferences((allDataInfluencedByConn)).GetMembersOfTarget();


//Explicit functions
result.Add(methods.FindByShortName("execute*"));
result.Add(methods.FindByShortName("insert"));
result.Add(methods.FindByShortName("create*"));
result.Add(methods.FindByShortName("add_property"));
result.Add(methods.FindByShortName("append"));
result.Add(methods.FindByShortName("*join"));

//Explicit session functions
result.Add(methods.FindByShortName("query").GetMembersOfTarget().FindByShortName("*join"));
result.Add(methods.FindByShortName("add"));
result.Add(methods.FindByShortName("save"));
result.Add(methods.FindByShortName("delete"));