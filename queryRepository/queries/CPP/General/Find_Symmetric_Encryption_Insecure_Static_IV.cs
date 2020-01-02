/*
Return flows between the key parameters of Symmetric Encryption functions to relevant sources
that are not sanitized, by safe random number generator.
Beside that it return also flows from the first parameter of memcpy that copy String Literals,
iv parameters of Symmetric Encryption that are not sanitized, by safe random number generator.
 
Parameters:
    param[0] - Memcpy first parameters
    param[1] - sanitizers
    param[2] - key parameters of Symmetric Encryption functions
*/

if(param.Length == 3) {
	CxList memcpyFirstParam = (CxList) param[0];
	CxList sanitizers = (CxList) param[1];
	CxList keyParams = (CxList) param[2];

	keyParams = Get_Variables_From_Addresses(keyParams);

	//Create a list of relevant sources that could be used for setting the IV
	CxList allRelevantSources = Find_Strings();	
	allRelevantSources.Add(All.FindDefinition(All.FindAllReferences(All.GetParameters(Find_Memory_Copy().FindByParameters(All.FindAllReferences(keyParams)), 1) - Find_Parameters())));
	
	CxList arrayRelevantSources = All.NewCxList();
	arrayRelevantSources.Add(All.FindByType(typeof(ArrayInitializer)).GetAncOfType(typeof(Declarator)));
	arrayRelevantSources.Add(Find_ObjectCreations().FindByShortName("DES_cblock", false).GetAncOfType(typeof(Declarator)));

	CxList StaticElements = All.FindByType(typeof(IntegerLiteral));
	StaticElements.Add(All.FindByType(typeof(RealLiteral)));
	StaticElements = StaticElements.GetByAncs(arrayRelevantSources);

	allRelevantSources.Add(arrayRelevantSources * StaticElements.GetAncOfType(typeof(Declarator)));
	
	//Add memcpy result and remove from parameters
	CxList memcpyResults = memcpyFirstParam.InfluencingOnAndNotSanitized(keyParams, sanitizers);
	keyParams -= memcpyResults.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

	//Get flows from Relevant Sources to Parameters and not sanitized
	CxList flowsNotSanitized = allRelevantSources.InfluencingOnAndNotSanitized(keyParams, sanitizers);
	//remove from remaining parameters the results found
	keyParams -= flowsNotSanitized.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

	//Get flows from Relevant Sources to Parameters sanitized to remove from remaining parameters
	CxList flowsSanitized = keyParams.DataInfluencedBy(allRelevantSources).ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);
	keyParams -= flowsSanitized.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
	
	result.Add(memcpyResults);
	result.Add(flowsNotSanitized);
}