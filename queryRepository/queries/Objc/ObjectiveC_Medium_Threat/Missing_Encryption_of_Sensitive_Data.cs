// Missing_Encryption_Sensitive_Data
// ---------------------------------
// The purpose of the query is as to find applications that allow the following:
//		Use non encrypted files while writing protected data.

// All insecure write, not including Log Write, because as insecure as Log is, we won't be encrypting the log output,
// so any data written to Log is found in a different query (not Encryption missing, but data Leakage).
CxList notRelevantOutput = Find_Log_Outputs();

result = Find_Insecure_Data_Storage(Find_Personal_Info(), notRelevantOutput);