// Query Stored Command Injection
// ==============================

CxList inputs = Find_Read();
inputs.Add(Find_DB_Out());

CxList exec = Find_Command_Injection_Outputs();

CxList sanitize = Find_Command_Injection_Sanitize();

result.Add(exec.InfluencedByAndNotSanitized(inputs, sanitize).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow));

CxList makeCommandInjection = Find_Make_Command_Injection();

result.Add(makeCommandInjection);