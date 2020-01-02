CxList sanitizer = Find_Sanitize();
sanitizer -= sanitizer.FindByShortName("setAttribute");

CxList urlEncodeSanitizer = All.FindByMemberAccess("URLEncoder.encode");
sanitizer.Add(urlEncodeSanitizer);

CxList secondSanitizer = Find_Sanitizer_For_Relience_On_Untusted_Input();
secondSanitizer.Add(urlEncodeSanitizer);

CxList inputs = Find_Source_Of_Security_Decision();
//System.getProperties should not be considered as input
CxList listSystemGetPropertiesInInputs = inputs.FindByMemberAccess("System.getProperty");
listSystemGetPropertiesInInputs.Add(inputs.FindByMemberAccess("System.getProperties"));
inputs -= listSystemGetPropertiesInInputs;

CxList ifAllCond = Find_Ifs();

CxList allVariables = Find_UnknownReference();

// calculate sink of security
CxList sink = Find_Sink_Of_Security_Decision();

//search direct flow from input to sink
CxList result_part_0 = inputs.InfluencingOnAndNotSanitized(sink, sanitizer, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);
//result_part_0 = result_part_0.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);

CxList varsOfResult_0 = All.NewCxList();

// if exists direct flow remove all its inputs
if (result_part_0.Count > 0)
{
	varsOfResult_0 = result_part_0.GetStartAndEndNodes(CxList.GetStartEndNodesType.AllNodes);
	CxList result_0_Inputs = result_part_0.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
	inputs = inputs - result_0_Inputs;
}
//at this moment, inputs are only those that do not directly influence sink of secutrity decision

// search for If conditions (all the elements in the condition)
CxList conditions = All.NewCxList();
foreach(CxList curIf in ifAllCond)
{
	try
	{
		IfStmt ifStmt = curIf.TryGetCSharpGraph<IfStmt>();
		CxList cond = All.NewCxList();
		cond.Add(ifStmt.Condition.NodeId, ifStmt.Condition);
		conditions.Add(All.GetByAncs(cond));
	}
	catch (Exception e)
	{
		cxLog.WriteDebugMessage(e.Message);
	}
}

//now get the variables, parameters and member accesses from all the elements in the conditions of ifs
CxList relevant_objects = All.NewCxList();
relevant_objects.Add(allVariables);
relevant_objects.Add(Find_Params());
relevant_objects.Add(Find_MemberAccess());

conditions = conditions * relevant_objects;

// first part. path from input to conditions with sanitizer
CxList part1 = inputs.InfluencingOnAndNotSanitized(conditions, sanitizer, CxList.InfluenceAlgorithmCalculation.NewAlgorithm).
	ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);




//all the variables except those that are in flows from input to sinks of security decision
allVariables -= varsOfResult_0;

//initialize lists to obtain results of other parts of the query
CxList result_part_1 = All.NewCxList();
CxList result_part_2 = All.NewCxList();

CxList alreadyDone = All.NewCxList();
// for each path from input to condition
foreach(CxList cxPath in part1.GetCxListByPath())  
{
	// get all ifs that are influenced by the inputs
	CxList ifActual_1 = cxPath.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
	
	//here we take only one path through each condition. eliminating parallel paths
	CxList test = alreadyDone * ifActual_1;
	if (test.Count > 0)
	{
		continue;
	}

	alreadyDone.Add(ifActual_1);
	
	foreach (CxList ifAct in ifActual_1)
	{
		CxList ifActual = ifAct.GetAncOfType(typeof(IfStmt));
		CxList result_1 = All.NewCxList();

		// find variables in true/false block of if (that are not in flows from input to sinks)
		CxList variables = allVariables.GetByAncs(ifActual);

		// calculate path from variables to sink
		CxList part2 = variables.InfluencingOnAndNotSanitized(sink, secondSanitizer)
			.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);

		//remove unnecessary variables within ifs 
		//this is, we get all those that do not go to sink of security decision
		CxList tempVar = part2.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);	
		variables = variables - tempVar;
	
		// connect input->condition //> variable->sink
		result_1 = cxPath.ConcatenateAllPaths(part2, false);
		result_part_1.Add(result_1);

		// search condition that are influenced by variables that do not go to sink of security decision
		CxList part3 = variables.InfluencingOnAndNotSanitized(conditions, secondSanitizer)
			.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);

		foreach(CxList onePath in part3.GetCxListByPath())              
		{
			CxList ifActual_2 = onePath.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);	
			ifActual_2 = ifActual_2.GetAncOfType(typeof(IfStmt));
			ifActual_2 -= ifActual;
			//get all the sinks of security decision within ifs (conditions + true + false) 
			CxList temp = sink.GetByAncs(ifActual_2);
			
			//connect variables (that do not go to sinks of security decisions)
			//to sinks of security decisions that are within ifs condition + true + false
			CxList tempResult1 = onePath.ConcatenateAllPaths(temp, false);
			//concatenate inputs->conditions >>> variables->conditions >>> sinks (in conditions + true + false)
			CxList tempResult = cxPath.ConcatenateAllPaths(tempResult1, false);
			result_part_2.Add(tempResult);
			
		}
	}
}

//final result is then:
//1 (part0) - inputs -> sinks
//2 (part1) - inputs->conditions >> variables->sinks
//3 (part2) - inputs->conditions >> variables(that are not in flows from input to sink)->conditions >> sinks (in ifs)
result = result_part_1;
result.Add(result_part_2);
result = result.ReduceFlowByPragma().ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);

result.Add(result_part_0);