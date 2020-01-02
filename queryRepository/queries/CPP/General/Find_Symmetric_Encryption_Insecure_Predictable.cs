/*
Parameters:
    param[0] - sanitizers
    param[1] - key parameters or IV parameters of Symmetric Encryption functions
*/

if(param.Length == 2) 
	{
	CxList sanitizers = (CxList) param[0];
	CxList parameters = (CxList) param[1];
	
	// Creates a list of irrelevant sources (that are covered by the Static IV query)	
	CxList allIrrelevantSources = Find_Strings();	
	allIrrelevantSources.Add(All.FindDefinition(All.FindAllReferences(All.GetParameters(Find_Memory_Copy().FindByParameters(All.FindAllReferences(parameters)), 1) - Find_Parameters())));
	
	CxList arrayIrrelevantSources = All.NewCxList();
	arrayIrrelevantSources.Add(All.FindByType(typeof(ArrayInitializer)).GetAncOfType(typeof(Declarator)));
	arrayIrrelevantSources.Add(Find_ObjectCreations().FindByShortName("DES_cblock", false).GetAncOfType(typeof(Declarator)));

	CxList StaticElements = All.FindByType(typeof(IntegerLiteral));
	StaticElements.Add(All.FindByType(typeof(RealLiteral)));
	StaticElements = StaticElements.GetByAncs(arrayIrrelevantSources);

	allIrrelevantSources.Add(arrayIrrelevantSources * StaticElements.GetAncOfType(typeof(Declarator)));
	CxList flowsToIgnore = allIrrelevantSources.DataInfluencingOn(parameters);
		
	//Ignores the irrelevant sources
	parameters -= flowsToIgnore.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

	//Get flows from objects to Parameters and not sanitized
	CxList flowsNotSanitized = All.FindDefinition(parameters).InfluencingOnAndNotSanitized(parameters, sanitizers);
	//remove from remaining parameters the results found
	parameters -= flowsNotSanitized.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
	CxList allRelevantSources = Find_UnknownReference();
	allRelevantSources.Add(Find_MemberAccesses());
	allRelevantSources.Add(Find_Declarators());			

	//Get flows from String Literals to Parameters sanitized to remove from remaining parameters 
	CxList flowsSanitized = allRelevantSources.DataInfluencingOn(parameters);
	parameters -= flowsSanitized.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
	
	result.Add(flowsNotSanitized);
	result.Add(parameters);
		
}