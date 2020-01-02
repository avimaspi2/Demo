CxList allDataInfluencedByConn = Find_DB_Conn_Postgres();

CxList methods = Find_Methods();
CxList allSelects = All.FindByShortName("*select*",false);

//Explicit fetch methods
CxList sink = methods.FindByShortName("fetch*");

//Explicit execute methods
sink.Add(methods.FindByShortName("execute*").FindByParameters(allSelects));

//Explicit psycopg methods
sink.Add(methods.FindByShortName("copy_from"));

//Explicit pygresql methods
sink.Add(methods.FindByShortName("getline"));
sink.Add(methods.FindByShortName("getlo"));

//Explicit pygresql pgquery object
sink.Add(methods.FindByShortName("query").FindByParameters(allSelects));

//Explicit pglarge object
sink.Add(methods.FindByShortName("write"));
sink.Add(methods.FindByShortName("export"));

result = sink.DataInfluencedBy(allDataInfluencedByConn).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);