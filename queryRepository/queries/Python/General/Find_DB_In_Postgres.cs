CxList allDataInfluencedByConn = Find_DB_Conn_Postgres();

CxList methods = Find_Methods();
CxList allSelects = All.FindByShortName("*select*", false);

CxList sink = All.NewCxList();

//Explicit execute methods
CxList execute = methods.FindByShortName("execute*");
sink.Add(execute);

//Explicit psycopy methods
sink.Add(methods.FindByShortName("mogrify"));
sink.Add(methods.FindByShortName("copy_from"));
sink.Add(methods.FindByShortName("copy_expert"));

//Explicit pygresql methods
CxList query = methods.FindByShortName("query");
sink.Add(query - query.FindByParameters(allSelects));

//Explicit insert methods
sink.Add(methods.FindByShortName("inserttable"));
sink.Add(methods.FindByShortName("putline"));
sink.Add(methods.FindByShortName("loimport"));

//Explicit pygresql DB alias
sink.Add(methods.FindByShortName("insert"));
sink.Add(methods.FindByShortName("update"));

//Explicit py-postgresql module
sink.Add(methods.FindByShortName("prepare"));
sink.Add(methods.FindByShortName("do"));

result = sink.DataInfluencedBy(allDataInfluencedByConn).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);