// Query Side_Chanel_Data_Leakage (Privacy_Violation)
// Find logging of unencrypted personal data
/////////////////////////////////////////////////////

CxList logOutputs = Find_Log_Outputs();
CxList personal = Find_All_Passwords() + Find_Personal_Info();
CxList sanitize = Find_Encrypt();

result = personal.InfluencingOnAndNotSanitized(logOutputs, sanitize);