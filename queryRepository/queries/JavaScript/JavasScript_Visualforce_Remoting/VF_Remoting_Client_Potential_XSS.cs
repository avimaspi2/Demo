//For apex 
CxList inputs = Find_Visualforce_Remoting_Inputs();

CxList outputs = Find_Outputs_XSS();

CxList sanitize = Sanitize();
sanitize.Add(Find_Visualforce_Remoting_Sanitize());
sanitize.Add(Find_XSS_Sanitize());

result = outputs.InfluencedByAndNotSanitized(inputs, sanitize);

result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);