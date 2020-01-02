///////////////////////////////////////////
//		Signed_Memory_Arithmetic
// Find memory allocation which can cause buffer overflow
// due of usage in signed variables.
///////////////////////////////////////////

CxList vulnerablePaths = All.NewCxList();
try
{	
	//Finds all the parameter that can cuse the signed memory arithmetic
	
	//1. malloc, calloc and realloc allocation 
	CxList vulnerableParameters = All.NewCxList();
	CxList parametersDefinitions = All.NewCxList();
	
	CxList methods = Find_Methods();
	CxList mallocs = methods.FindByName("malloc");
	
	List<string> methodsNames = new List<string> {"realloc", "calloc"};
	CxList reallocsAndCallocs = methods.FindByShortNames(methodsNames);	
	
	CxList allocationParameters = All.GetParameters(mallocs, 0);
	allocationParameters.Add(All.GetParameters(reallocsAndCallocs, 1));
	vulnerableParameters.Add(allocationParameters);
	
	//2. arrays allocation
	CxList arrays = Find_ArrayCreateExpr();
	CxList methodInvokeVulnerableParameters = All.NewCxList();
	CxList unknownReferenceVulnerableParameters = All.NewCxList();
	foreach(CxList array in arrays){
		CxList arrayAnces = All.GetByAncs(array);
		CxList methodInvoke = arrayAnces.FindByType(typeof(MethodInvokeExpr));
		if (methodInvoke.Count > 0)
			methodInvokeVulnerableParameters.Add(methodInvoke);
		else
			unknownReferenceVulnerableParameters.Add(arrayAnces.FindByType(typeof(UnknownReference)));
	}
	string[] unsignedTypes = new string[] {"char", "uint*", "UInt*", "NSUInteger", "size_t"};
	unknownReferenceVulnerableParameters -= Find_Integers().FindByTypes(unsignedTypes);
	parametersDefinitions = All.FindDefinition(unknownReferenceVulnerableParameters);

	vulnerableParameters.Add(methodInvokeVulnerableParameters);
	vulnerableParameters.Add(unknownReferenceVulnerableParameters);

	CxList explicitSignedDefinitions = parametersDefinitions.FindByExtendedType("signed");
	CxList potenialUnsignedDefinitions = parametersDefinitions.FindByTypes(unsignedTypes);
	
	parametersDefinitions -= potenialUnsignedDefinitions;	
	parametersDefinitions.Add(explicitSignedDefinitions);		
	parametersDefinitions -= parametersDefinitions.FindByExtendedType("unsigned");
	vulnerableParameters -= vulnerableParameters.FindByShortName("sizeof");
	foreach (CxList parameter in vulnerableParameters.GetCxListByPath())
	{
		CxList vulnerablePath = All.NewCxList();
		if(parameter.FindByType(typeof(MethodInvokeExpr)).Count > 0)
			vulnerablePath.Add(parameter);		
		else{
			vulnerablePath = parametersDefinitions.FindDefinition(parameter).ConcatenatePath(parameter);
		}
		CxList endNodes = vulnerablePath.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);		
		endNodes -= endNodes.FindByShortName("count*");
		if (endNodes.Count > 0) 
		{
			vulnerablePaths.Add(vulnerablePath);
		}
	}
}
catch (Exception error)
{
	cxLog.WriteDebugMessage(error);
}
result = vulnerablePaths;