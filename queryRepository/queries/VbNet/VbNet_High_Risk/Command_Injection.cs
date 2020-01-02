CxList inputs = Find_Interactive_Inputs();
CxList exec = Find_Command_Execution();
CxList sanitize = Find_Command_Injection_Sanitize();
result = exec.InfluencedByAndNotSanitized(inputs, sanitize);