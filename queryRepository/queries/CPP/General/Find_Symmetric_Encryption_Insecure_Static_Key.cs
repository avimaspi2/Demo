/*
Return flows between the key parameters of Symmetric Encryption functions to String Literals
that are not sanitized, by safe random number generator.
Beside that it return also flows from the first parameter of memcpy that copy String Literals,
key parameters of Symmetric Encryption that are not sanitized, by safe random number generator.
 
Parameters:
    param[0] - Memcpy first parameters
    param[1] - sanitizers
    param[2] - key parameters of Symmetric Encryption functions
*/
if(param.Length == 3) {
	CxList memcpyFirstParam = (CxList) param[0];
	CxList sanitizers = (CxList) param[1];
	CxList keyParams = (CxList) param[2];

	//Add memcpy result and remove from parameters
	CxList memcpyResults = memcpyFirstParam.InfluencingOnAndNotSanitized(keyParams, sanitizers);
	keyParams -= memcpyResults.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
	
	//Get flows from String Literals to Parameters and not sanitized
	CxList flowsNotSanitized = Find_Strings().InfluencingOnAndNotSanitized(keyParams, sanitizers);
	//remove from remaining parameters the results found
	keyParams -= flowsNotSanitized.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
	//Get flows from String Literals to Parameters sanitized to remove from remaining parameters
	CxList flowsSanitized = Find_Strings().DataInfluencingOn(keyParams);
	keyParams -= flowsSanitized.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
	
	result.Add(memcpyResults);
	result.Add(flowsNotSanitized);
	result.Add(keyParams);
}