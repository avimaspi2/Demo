//finds command injection
CxList dynamic_shell = Find_Command_Execution();
CxList sanitize = Find_Command_Injection_Sanitize();
CxList inputs = Find_Interactive_Inputs();

result = inputs.InfluencingOnAndNotSanitized(dynamic_shell, sanitize);