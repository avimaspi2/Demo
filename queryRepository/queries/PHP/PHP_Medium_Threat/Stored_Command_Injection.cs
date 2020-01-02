//finds command injection
//CxList methods = Find_Methods();

CxList dynamic_shell = Find_Command_Execution();
CxList sanitize = Find_Command_Injection_Sanitize();
CxList db = Find_DB_Out() + Find_Read();

result = db.InfluencingOnAndNotSanitized(dynamic_shell, sanitize);