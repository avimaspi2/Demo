/* This query is meant to detect 'Command Injection' vulnerability 
 * Refer to:
 *     https://cwe.mitre.org/data/definitions/78.html
 *     https://cwe.mitre.org/data/definitions/77.html
 *     https://cwe.mitre.org/data/definitions/88.html
 */

CxList inputs = Find_Interactive_Inputs();
CxList outputs = Find_Cmd_Execute();
CxList sanitize = Find_Command_Injection_Sanitize();
result = outputs.InfluencedByAndNotSanitized(inputs, sanitize);