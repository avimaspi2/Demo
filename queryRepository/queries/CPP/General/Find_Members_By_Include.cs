/*
	DESCRIPTION:
	Query to find Method Invokes used in files that have a specific import.

	INPUT:	
	packageName 	- The include header name.
	methods 		- The List of methods to search for.

	RETURN: All methods/member access occurrences.
*/
if (param.Length == 2)
{
	string packageName = param[0] as string;
	String[] methods = param[1] as string[]; 
		
	CxList lib = Find_Strings().FindByShortName(packageName);
	CxList allIncludes = Find_Methods().FindByShortName("CX_INCL");
	CxList includesWithLib = lib.GetParameters(allIncludes, 0);
	CxList filesWithLib = All.FindByFiles(includesWithLib).FindByType(typeof(MethodInvokeExpr));
	result = filesWithLib.FindByShortNames(new List<string>(methods));
}
else 
{
	cxLog.WriteDebugMessage("Number of parameters should be 2");	
}