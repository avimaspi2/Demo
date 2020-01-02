CxList inputs = NodeJS_Find_Interactive_Inputs();
CxList logOutputs = Hapi_Find_Log_Write();
CxList sanitize = NodeJS_Find_General_Sanitize();

result = inputs.InfluencingOnAndNotSanitized(logOutputs, sanitize);