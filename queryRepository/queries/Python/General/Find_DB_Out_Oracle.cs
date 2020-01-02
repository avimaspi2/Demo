CxList allDataInfluencedByConn = All.FindAllReferences(Find_DB_Conn_Oracle());
CxList methods = allDataInfluencedByConn.GetMembersOfTarget();

//Explicit functions
result.Add(methods.FindByShortName("fetch*"));
result.Add(methods.FindByShortName("var"));

//Implicit fecth
result.Add(allDataInfluencedByConn.GetFathers().FindByType(typeof(ForEachStmt)));