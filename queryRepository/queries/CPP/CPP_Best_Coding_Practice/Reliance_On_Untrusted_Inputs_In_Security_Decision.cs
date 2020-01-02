CxList sanitizer = Find_Sanitize();
CxList inputs    = Find_Source_Of_Security_Decision();
CxList ifAllCond = Find_Ifs();

// calculate sink of security
CxList sink = Find_Sink_Of_Security_Decision();

//search direct flow from input to sink
CxList result_0 = inputs.InfluencingOnAndNotSanitized(sink, sanitizer, CxList.InfluenceAlgorithmCalculation.NewAlgorithm).
	ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);

CxList varsOfResult_0 = result_0.GetStartAndEndNodes(CxList.GetStartEndNodesType.AllNodes);

// if exists direct flow remove all its inputs
if (result_0.Count > 0)
{
	CxList result_0_Inputs = result_0.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
	inputs = inputs - result_0_Inputs;
}


// search for If conditions
CxList conditions = All.NewCxList();
foreach(CxList curIf in ifAllCond)
{
	IfStmt ifStmt = curIf.data.GetByIndex(0) as IfStmt;
	CxList cond = All.NewCxList();
	cond.Add(ifStmt.Condition.NodeId, ifStmt.Condition);
	conditions.Add(All.GetByAncs(cond));
}

// first part. path from input to conditions with sanitizer
CxList part1 = inputs.InfluencingOnAndNotSanitized(conditions, sanitizer, CxList.InfluenceAlgorithmCalculation.NewAlgorithm).
	ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);

CxList secondSanitizer = Find_Sanitizer_For_Relience_On_Untusted_Input();

CxList allVariables = Find_UnknownReference();

allVariables -= varsOfResult_0; // remove all variables that already on existing path

// for each path from input to condition
foreach(CxList cxPath in part1.GetCxListByPath())  
{
	// find all if that influenced by input
	CxList ifActual_1 = cxPath.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

	foreach (CxList ifAct in ifActual_1)
	{
		CxList ifActual = ifAct.GetAncOfType(typeof(IfStmt));
		CxList result_1 = All.NewCxList();

		// find variables in true/false block of if
		CxList variables = allVariables.GetByAncs(ifActual);

		// calculate path from variables to sink
		CxList part2 = variables.InfluencingOnAndNotSanitized(sink, secondSanitizer).
								ReduceFlowByPragma().
								ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);

		// remove unnecessary variables
		CxList tempVar = part2.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);	
		variables = variables - tempVar;
	
		// connect input->condition //> variable->sink
		result_1 = cxPath.ConcatenateAllPaths(part2);
		result.Add(result_1);

		// search condition  than influenced by variables
		CxList part3 = variables.InfluencingOnAndNotSanitized(conditions, secondSanitizer).
			ReduceFlowByPragma().
			ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);

		foreach(CxList onePath in part3.GetCxListByPath())              
		{
			CxList ifActual_2 = onePath.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);	
			ifActual_2 = ifActual_2.GetAncOfType(typeof(IfStmt));
			ifActual_2 -= ifActual;
			CxList temp = sink.GetByAncs(ifActual_2);
			
			CxList tempResult1 = onePath.ConcatenateAllPaths(temp).ReduceFlowByPragma();
			CxList tempResult = cxPath.ConcatenateAllPaths(tempResult1);
			result.Add(tempResult);
		}
	}
}
result = result + result_0;
result = result.ReduceFlowByPragma().ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);