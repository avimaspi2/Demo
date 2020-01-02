/**
This query should yield results whenever sensitive information is stored without encryption.	
**/
//Find "sensitive inputs"
//credit card numbers, social security numbers, names, addresses, ages, etc.
CxList secrets = Find_Personal_Info();
//Find sinks 
//	DB, File, other storage types
CxList outputs = All.NewCxList();
outputs.Add(Find_DB_In());
outputs.Add(Find_Write());
outputs.Add(Find_Log_Outputs());
outputs.Add(Find_Outputs());
outputs.Add(Find_Bounded_Methods_Source_Parameters());

//Sanitizers
//	Symmetric ciphers influencing the data (e.g.AES(key, social_security_number) )
CxList sanitizers = Find_Encrypt();
CxList plainInfo = secrets.InfluencingOnAndNotSanitized(outputs, sanitizers);
result = plainInfo.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);