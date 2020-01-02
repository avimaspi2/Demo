///////////////////////////////////////////////////
//		Plaintext_Storage_Of_A_Password
// Password management issues occur when a password
// is stored in plaintext in an application's properties 
// or configuration file.
// This query finds inputs retrive from storage,
// that doesn't go through sanitizer and used to connect to DB. 
// https://cwe.mitre.org/data/definitions/256.html
//////////////////////////////////////////////////
CxList passwords = Find_All_Passwords();
CxList outputs = All.NewCxList();
outputs.Add(Find_DB_In());
outputs.Add(Find_Write());
outputs.Add(Find_Log_Outputs());
outputs.Add(Find_Outputs());


CxList hashing = Find_Hash_Functions_Outputs();

CxList whitePasses = passwords.InfluencingOnAndNotSanitized(outputs, hashing);

result = whitePasses.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);