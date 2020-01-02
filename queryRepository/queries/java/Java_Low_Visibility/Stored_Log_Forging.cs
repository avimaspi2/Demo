/*
* The application loads untrusted data, that may have earlier been tampered with by a malicious user. 
* This data is then used, without sufficient validation or sanitization, to format and write a message
* to the logging mechanism. 
*/
CxList inputs = Find_DB();
CxList logOutputs = Find_Log_Outputs();

CxList sanitize = Find_Splitting_Sanitizer();

result = logOutputs.InfluencedByAndNotSanitized(inputs, sanitize);