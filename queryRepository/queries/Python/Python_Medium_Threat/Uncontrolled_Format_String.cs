CxList inputs = Find_Inputs();
CxList methods = Find_Methods();

CxList print = methods.FindByShortName("print");
CxList sanitizers = Find_Integers();

result = print.InfluencedByAndNotSanitized(inputs, sanitizers);
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);