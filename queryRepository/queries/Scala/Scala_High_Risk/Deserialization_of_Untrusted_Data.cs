CxList inputs = Find_Deserialization_Inputs();
CxList deserializers = Find_Unsafe_Deserializers();
CxList sanitizers = Find_Deserialization_Sanitizers();

//remove all the deserializers in coditioned blocks where the condition is influenced by hash
CxList allCond = Find_Conditions();

CxList comparison = All.GetByAncs(allCond).FindByShortName("==");
comparison.Add(All.GetByAncs(allCond).FindByShortName("!="));

CxList relevantComparison = comparison.InfluencedBy(sanitizers);
relevantComparison = comparison.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

CxList relevantIfs = relevantComparison.GetAncOfType(typeof(IfStmt));
relevantIfs.Add(relevantComparison.GetAncOfType(typeof(IterationStmt)));

deserializers -= deserializers.GetByAncs(relevantIfs);
result.Add(inputs.InfluencingOnAndNotSanitized(deserializers, sanitizers).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow));

CxList notCatchingFlow = deserializers - deserializers.Contained(result, CxList.GetStartEndNodesType.AllNodes);

result.Add(notCatchingFlow);