CxList inputs = Find_Read()+ Find_DB_Out()  + 
	All.FindByMemberAccess("Process.GetProcesses");

CxList exec = Find_Command_Execution();
CxList sanitize = Find_Sanitize();

result = exec.InfluencedByAndNotSanitized(inputs, sanitize);