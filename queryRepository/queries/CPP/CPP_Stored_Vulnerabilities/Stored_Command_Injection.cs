CxList inputs = Find_Read()+Find_DB();
CxList exec = 	Find_Cmd_Execute();
CxList sanitize = Find_Command_Injection_Sanitize();

result = exec.InfluencedByAndNotSanitized(inputs, sanitize);