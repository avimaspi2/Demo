// This query finds string compares that deffintly aren't checking a filePath.
// We assume we're getting all the relevent string-compares as a parameter

if (param.Length == 1)
{
	CxList compares = param[0] as CxList;
	
	CxList compareParams = Find_Strings().GetParameters(compares);
	CxList innocentParams = compareParams.FindByShortName("null", false);
	innocentParams.Add(compareParams.FindByShortName("utf-8", false));
	innocentParams.Add(compareParams.FindByShortName("utf-16", false));
	innocentParams.Add(compareParams.FindByShortName("utf-32", false));
	CxList innocentCompares = compares.FindByParameters(innocentParams);

	result = innocentCompares;
}