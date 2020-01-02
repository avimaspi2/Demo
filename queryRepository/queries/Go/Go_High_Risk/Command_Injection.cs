/*
 * This query finds command injection by:
 *  - Firstly searching for inputs that can be used as commands to be executed
 *  - Get possible sanitizers to filter the final result
 *  - Then searches for command executions
 *	- If there are inputs that change the command executions, the query will return that flow.
*/

/*** Finding inputs and sanitizers ***/
CxList inputs = Find_Inputs();
CxList sanitizers = Find_Command_Injection_Sanitize();

/*** Finding exec.Command executions ***/
CxList outputs = Find_Exec_Outputs();

/*** Finding syscall.Exec executions ***/
outputs.Add(All.FindByMemberAccess("\"syscall\".Exec"));

/*** Finding all executions influenced by inputs, but not by sanitized inputs ***/
CxList cmdInjection = outputs.InfluencedByAndNotSanitized(inputs, sanitizers).ReduceFlowByPragma();
result = cmdInjection.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);