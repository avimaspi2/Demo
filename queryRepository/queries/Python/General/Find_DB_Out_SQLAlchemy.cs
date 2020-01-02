CxList allDataInfluencedByConn = All.FindAllReferences(Find_DB_Conn_SQLAlchemy());
CxList methods = allDataInfluencedByConn.GetMembersOfTarget();

//Explicit functions
result.Add(methods.FindByShortName("fetch*"));
result.Add(methods.FindByShortName("select"));

//Implicit fecth
result.Add(allDataInfluencedByConn.GetFathers().FindByType(typeof(ForEachStmt)));

//Query methods
CxList queries = methods.FindByShortName("query").GetMembersOfTarget();
result.Add(queries - queries.FindByShortName("*join"));