CxList db = Find_DB_Output();
CxList inputs = Find_DB_Input();
CxList sanitized = Sanitize() + Find_Test_Code();
// Remove the inputs (db_out) from the sanitization, otherwise there are not results at all
CxList sanitizedInputs = sanitized * inputs;
sanitized = sanitized - sanitizedInputs;

result = inputs.InfluencingOnAndNotSanitized(db, sanitized).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);