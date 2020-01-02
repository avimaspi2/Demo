//Get Inputs
CxList inputs = Find_Interactive_Inputs();
//Get Console Outputs
CxList consoleOut = Find_Console_Outputs();

//Get All Params and filter out typeof Param and StringLiteral
CxList par = All.GetParameters(consoleOut);
par -= par.FindByType(typeof(Param));
par -= par.FindByType(typeof(StringLiteral));

CxList outputs = All.NewCxList();

foreach (CxList output in consoleOut)
{	//Get the specific parameters of output 
	CxList p = par.GetParameters(output);
	//If parameters exist
	if (p.Count > 0)
	{	//Join the path of the param with the output
		outputs.Add(p.ConcatenatePath(output, false));
	}
}

//Find Sanitized list
CxList sanitized = Find_XSS_Sanitize();

//Get the flow between outputs tha are influeced by inputs and are not sanitized
result = inputs.InfluencingOnAndNotSanitized(outputs, sanitized, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);

//Add the results that are both input and output but are not sanitized
 foreach (CxList output in outputs.GetCxListByPath())
{	//Get the start node that is also an input
	CxList pathStart = output.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly) * inputs;
	CxList pathEnd = output.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly); //Get end node
	//If the start node and end node are not a sanitizer
	if ((pathStart + pathEnd - sanitized).Count == 2)
	{	//If the path is not sanitized, then add to result
		result.Add(output);
	}
}