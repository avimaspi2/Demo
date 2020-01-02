CxList methods = Find_Methods();

CxList MSMQInput = Find_Deserialization_Inputs_MSMQ();
CxList binaryFormatter = All.FindByType("BinaryMessageFormatter");
CxList relevantBinaryFormatter = (MSMQInput.InfluencedBy(binaryFormatter)).GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);

CxList messageBody = All.FindByMemberAccess("Message.Body");
result.Add(messageBody.InfluencedBy(relevantBinaryFormatter).ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow));

CxList binaryFormatterRead = methods.FindByMemberAccess("BinaryMessageFormatter.Read");
result.Add(binaryFormatterRead.InfluencedBy(MSMQInput));