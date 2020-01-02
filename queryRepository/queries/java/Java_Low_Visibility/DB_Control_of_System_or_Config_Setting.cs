CxList setters = Find_Setters();

CxList sanitize = Find_General_Sanitize();
CxList inputs = Find_DB_Out();
inputs.Add(Find_Read_NonDB());

result = inputs.InfluencingOnAndNotSanitized(setters, sanitize);