CxList include = Find_File_Includes();

CxList db = Find_DB_Out();
db.Add(Find_Read());

// find the file input sanitizers (number and string functions)
CxList sanitized = Find_File_Sanitizers();

result = include.InfluencedByAndNotSanitized(db, sanitized);
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);