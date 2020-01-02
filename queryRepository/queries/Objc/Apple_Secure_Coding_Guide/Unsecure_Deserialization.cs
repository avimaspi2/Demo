// Find Unsecure Deserialization - origin from external input (not secured) and serialized by an unsecured method
CxList vulnerablePaths = All.NewCxList();
try
{
	
	CxList https = Find_HTTPS();
	CxList externalInputs = Find_Pure_Http_and_Downloaded_Data(); // include both http and https as external source
	externalInputs.Add(https);	
	
	CxList classDecl = Find_Classes();	
	CxList unsecureObjcWrite = Find_ObjC_Write().DataInfluencedBy(externalInputs);

	CxList initWithCoders = All.FindByShortName("*initWithCoder:");
	CxList nsKeyedUnarchiver = All.FindByShortName("NSKeyedUnarchiver");
	CxList nsKeyedUnarchiverMembers = nsKeyedUnarchiver.GetMembersOfTarget();
	
	
	CxList unarchiveObject = nsKeyedUnarchiverMembers.FindByShortName("unarchiveObject*");
	
	List<string> unarchiveObjWithDataMethNames = new List<string>{
			"unarchiveObjectWithData:",
			"unarchiveObject:with:"};
	
	CxList unarchiveObjectWithDataMethods = nsKeyedUnarchiverMembers.FindByShortNames(unarchiveObjWithDataMethNames);

	unarchiveObjectWithDataMethods.Add(unarchiveObject.FindByParameterName("with", 0));	
	
	List<string> unarchiveObjWithFileMethNames = new List<string>{
			"unarchiveObjectWithFile:",
			"unarchiveObject:withFile:"};	
	
	CxList unarchiveObjectWithFileMethods = nsKeyedUnarchiverMembers.FindByShortNames(unarchiveObjWithFileMethNames);
	unarchiveObjectWithFileMethods.Add(unarchiveObject.FindByParameterName("withFile", 0));

	CxList potenialVulnerableMethods = All.NewCxList();
	potenialVulnerableMethods.Add(initWithCoders);
	potenialVulnerableMethods.Add(unarchiveObjectWithDataMethods);
	
	CxList potenialVulnerableFlows = potenialVulnerableMethods.DataInfluencedBy(externalInputs);

	CxList securedClasses = Find_Secured_Classes();
	CxList insecureClasses = classDecl - securedClasses;
	
	// check if the flow impact directly on an insecure object
	foreach (CxList flow in potenialVulnerableFlows.GetCxListByPath())
	{
		CxList flowFathers = flow.GetFathers();
		CxList insecureClassImpacted = flowFathers.FindByType(insecureClasses);
		if (insecureClassImpacted.Count == flowFathers.Count) // check if the direct impact is insecured
		{
			CxList concat = flow.ConcatenatePath(insecureClassImpacted, false);
			vulnerablePaths.Add(concat);
		}
		else if (flowFathers.FindByType(classDecl).Count == 0) 
		// if the direct impact is not to a class object
		// in this case the impact cannot be checked to be a secured class so a vulnerability is suspected
		{
			vulnerablePaths.Add(flow);
		}
	}
		
	// check files deserializations
	CxList unarchiveObjectWithFileFlows = All.NewCxList();
	foreach (CxList flow in unarchiveObjectWithFileMethods.GetCxListByPath())
	{
		CxList fileFlow = All.GetParameters(flow, 0); // file name parameter
		fileFlow = All.DataInfluencedBy(fileFlow);
		
		CxList flowFathers = fileFlow.GetFathers();
		CxList insecureClassImpacted = flowFathers.FindByType(insecureClasses);
		if (insecureClassImpacted.Count == flowFathers.Count) // check if the direct impact is insecured
		{
			CxList concat = fileFlow.ConcatenatePath(insecureClassImpacted, false);
			unarchiveObjectWithFileFlows.Add(fileFlow); // unarchiveObjectWithFileMethods impact on insecure object
		}
		else if (flowFathers.FindByType(classDecl).Count == 0)
		// if the direct impact is not to a class object
		// in this case the impact cannot be checked to be a secured class so a vulnerability is suspected
		{
			unarchiveObjectWithFileFlows.Add(flow);
		}
	}
	// delete duplicated paths
	unarchiveObjectWithFileFlows = unarchiveObjectWithFileFlows.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);
	foreach (CxList flow in unarchiveObjectWithFileFlows.GetCxListByPath())
	{
		// The start node of the flow is the file itself
		CxList startNode = flow.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
		CxList startNodeDefinition = All.FindDefinition(startNode); // file definition
		foreach (CxList path in unsecureObjcWrite.GetCxListByPath())
		{
			CxList fileNameDefinition = All.FindDefinition(All.GetParameters(path.GetMembersOfTarget(), 0));
			// if the file write and file serialize defined in the same place
			if (startNodeDefinition == fileNameDefinition)
			{
				// build full flow (vulnerable path) - conact start part with end part of the vulnerable flow
				CxList concat = path.ConcatenatePath(flow, false);
				vulnerablePaths.Add(concat);
			}			
		}              
	}
}
catch (Exception error)
{
	cxLog.WriteDebugMessage(error);
}
result = vulnerablePaths.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);