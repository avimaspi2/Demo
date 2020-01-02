CxList inputs = Find_Read();
inputs.Add(Find_DB_Out());

CxList exec = Find_Command_Execution();

result = exec.DataInfluencedBy(inputs);