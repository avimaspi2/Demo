CxList read = Find_Read_NonDB();

CxList dbRead = Find_Read_DB();

result = read;
result.Add(dbRead);