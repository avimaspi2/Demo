// Insufficient_Sensitive_Transport_Layer - output
// -----------------------------------------------
// The purpose of the query is as to find applications that allow the following:
//		Use http instead of https while sending protected data.

CxList newOutputs = Find_Insufficient_Out_Transport_Layer();

CxList personalInfo = Find_Personal_Info();

result = newOutputs.DataInfluencedBy(personalInfo, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);

result = result.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);