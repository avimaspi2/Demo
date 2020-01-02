CxList logs = Find_Log_Outputs();
CxList personalInfo = Find_Personal_Info();
CxList sanitizers = Find_Integers();

result = personalInfo.InfluencingOnAndNotSanitized(logs, sanitizers);