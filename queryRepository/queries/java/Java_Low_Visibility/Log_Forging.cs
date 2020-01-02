CxList Inputs = Find_Interactive_Inputs();

CxList readLinesStored = Find_Readline_From_Stored();
Inputs -= readLinesStored;

CxList Log = Find_Log_Outputs();

CxList sanitize = Find_Splitting_Sanitizer();

result = Log.InfluencedByAndNotSanitized(Inputs, sanitize);