CxList relevantOutput = Find_Outputs_XSRF();
CxList inputs = Find_Visualforce_Remoting_Inputs();
CxList sanitize = Sanitize();
result = relevantOutput.InfluencedByAndNotSanitized(inputs, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);
result.Add(Find_Source_Equals_Sink(inputs, relevantOutput));