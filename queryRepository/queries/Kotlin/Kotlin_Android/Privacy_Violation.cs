// This query searches for variables and constants that could contain personal sensitive data which is streamed to an output.

// Find personal information
CxList personalInfo = Find_Personal_Info() - Find_Strings();

// Find inputs
CxList inputs = Find_Inputs(); 
inputs.Add(Find_DB_Out());

// Find outputs
CxList outputs = Find_Outputs();

// Define sanitize
CxList sanitize = Find_Sanitize();
sanitize.Add(Find_DB_In()); // in some languages is called Find_DB, Find_DB_In, Find_DB_Input
sanitize.Add(Find_Encrypt());

CxList personalInfoClone = personalInfo.Clone();
personalInfo = personalInfo.DataInfluencedBy(inputs).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
personalInfo.Add(personalInfoClone * inputs);
personalInfo.Add(personalInfoClone * inputs.GetTargetOfMembers());

// Byte arrays are considered sanitized
personalInfo -= personalInfo.FindByType("ByteArray");

// find all variables that are influencing an output
result  = outputs.InfluencedByAndNotSanitized(personalInfo, sanitize);

result = result.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);