CxList exequteSQL = Find_Methods().FindByShortName("executeSql");
CxList unknownReferences = Find_UnknownReference();
CxList parameters = All.GetParameters(exequteSQL, 1);
result.Add(parameters);
result.Add(unknownReferences.GetByAncs(parameters));
result.Add(Find_Indexed_DB_In());