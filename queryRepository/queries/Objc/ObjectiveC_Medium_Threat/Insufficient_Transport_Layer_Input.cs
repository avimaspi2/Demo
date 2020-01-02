// Insufficient_Sensitive_Transport_Layer - input
// ----------------------------------------------
// The purpose of the query is as to find applications that allow the following:
//		Use http instead of https while receiving protected data.

CxList personalInfo = Find_Personal_Info();
CxList unsecureInputs = Find_Pure_Http_and_Downloaded_Data();

result = personalInfo.DataInfluencedBy(unsecureInputs, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);