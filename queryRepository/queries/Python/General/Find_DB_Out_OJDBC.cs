CxList allDataInfluencedByConn = All.FindAllReferences(Find_DB_Conn_OJDBC());
CxList methods = allDataInfluencedByConn.GetMembersOfTarget();

//Explicit functions
result.Add(methods.FindByShortName("fetch*"));

//Implicit fecth
result.Add(allDataInfluencedByConn.GetFathers().FindByType(typeof(ForEachStmt)));