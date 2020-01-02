/***** Query to find the non mapped Handlebars template invocations *****/

if(cxScan.IsFrameworkActive("Handlebars"))
{
	CxList invokes = Find_Methods();

	// Find template functions returned by Handlebars.compile
	CxList templateFunctions = invokes.FindByMemberAccess("Handlebars.compile").GetAssignee();

	// Find where template functions are being invoked
	CxList viewCalls = invokes.FindAllReferences(templateFunctions);

	CxList viewCallsWithDefinition = All.NewCxList();

	foreach (CxList viewCall in viewCalls)
	{
		int count = (All.FindDefinition(viewCall)).Count;
	
		// If greater than 1 it means that other than its own definition, it has a handlebars template definition
		if (count > 1)
		{
			viewCallsWithDefinition.Add(viewCall);
		}
	}

	// Get all template calls with indexer references
	// ex: Handlebars.templates[key](data);
	CxList templateCalls = invokes.FindByName("*Handlebars.templates");
	CxList indexerRefs = Find_IndexerRefs();
	CxList templateCallsWithIndexerRefs = templateCalls * indexerRefs.GetAncOfType(typeof(MethodInvokeExpr));

	// Filter only template calls with unknown references as an indexer
	CxList unkRefs = Find_UnknownReference();
	CxList templateCallsWithUnknownIndexerRefs = templateCallsWithIndexerRefs * unkRefs.GetAncOfType(typeof(MethodInvokeExpr));

	viewCalls.Add(templateCallsWithUnknownIndexerRefs);

	result = viewCalls - viewCallsWithDefinition;
}