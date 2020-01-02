CxList inputs = general.Find_Deserialization_Inputs();
inputs.Add(general.Find_Deserialization_Inputs_Language());
CxList deserializers = general.Find_Unsafe_Deserializers();
CxList sanitizers = general.Find_Deserialization_Sanitizers();

//remove all the deserializers in coditioned blocks where the condition is influenced by hash
CxList allCond = general.Find_Conditions();

CxList comparison = All.GetByAncs(allCond).FindByShortName("==");
comparison.Add(All.GetByAncs(allCond).FindByShortName("!="));

CxList relevantComparison = comparison.InfluencedBy(sanitizers);
relevantComparison = comparison.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

CxList relevantIfs = relevantComparison.GetAncOfType(typeof(IfStmt));
relevantIfs.Add(relevantComparison.GetAncOfType(typeof(IterationStmt)));

deserializers -= deserializers.GetByAncs(relevantIfs);

result.Add(inputs.InfluencingOnAndNotSanitized(deserializers, sanitizers).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow));