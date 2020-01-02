/* 
 * This query is meant to detect 'Cross-site Scripting' vulnerability (https://cwe.mitre.org/data/definitions/79.html)
 * in the case that the potentially malicious content comes from stored files or databases
*/

if (CGI().Count > 0) 
{
	CxList sanitize = Find_CGI_Sanitize();
	CxList stored = Find_DB();
	stored.Add(Find_Read());
	CxList outputs = Find_Outputs();
	result = stored.InfluencingOnAndNotSanitized(outputs, sanitize);
}