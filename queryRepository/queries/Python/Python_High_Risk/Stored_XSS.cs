CxList db = Find_DB_Out();
CxList read = Find_Read();

CxList inputs = All.NewCxList();
inputs.Add(db);
inputs.Add(read);

CxList outputs = Find_XSS_Outputs();
CxList sanitize = Find_XSS_Sanitize();

result = inputs.InfluencingOnAndNotSanitized(outputs, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);
/*	XSS can be sanitized in templates, not covered. */