// Find Path Manipulations
// Make sure filenames and other urls are not constructed from data received from open-url without sanitation.
CxList vulnerbaleResults = All.NewCxList();
try
{
	CxList methods = Find_Methods();	
	CxList sources = Find_Inputs();
	CxList sanitize = Find_Sanitize();
	CxList pathSanitize = Path_Sanitize();
	CxList sanitizers = All.NewCxList();
	sanitizers.Add(sanitize);
	sanitizers.Add(pathSanitize);
	
	CxList compareMethods = All.NewCxList();
	
	List<string> methodsNames = new List<string>{"caseInsensitiveCompare*",
			"localizedCaseInsensitiveCompare*", "compare*", "localizedCompare*",
			"localizedStandardCompare*","isEqualToString*","*isEqual*"
			};
	
	compareMethods.Add(methods.FindByShortNames(methodsNames));
	
	// filter sanitized sources which uses compare methods
	sources -= sources.InfluencingOn(compareMethods.GetTargetOfMembers()).GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);		
	CxList fileReferences = Find_File_URL_Direct_Ref();	
	vulnerbaleResults = fileReferences.InfluencedByAndNotSanitized(sources, sanitizers);	
}
catch (Exception error)
{
	cxLog.WriteDebugMessage(error);
}
result = vulnerbaleResults.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);