/// <summary>
/// Find information exposure upon exception.
/// Check whether exception information (both attributes and methods) are going into 
/// output without sanitization.
/// </summary>

CxList exp = Find_Exception_Information();
CxList outputs = Find_Outputs();
CxList sanitize = Find_Integers();

// Results are all flows from data to output + all exceptions, which are output parameters
result.Add(outputs.InfluencedByAndNotSanitized(exp, sanitize));
result.Add(outputs * exp);
result = result.ReduceFlowByPragma().ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);