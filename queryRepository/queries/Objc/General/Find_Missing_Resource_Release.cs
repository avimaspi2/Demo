/* 
  DESCRIPTION
  Find_Missing_Resource_Release(openProc, openIndx, closeProc, closeIndx)
  looks for resources that are allocated by 'openProc' but not deallocated
  by 'closeProc'. Both 'openProc' and 'closeProc' stand for names of
  procedures (strings).

  The 'openIndx' and 'closeIndx' (integers) parameters configure where the
  identifier holding the resource shall be found with respect to the invocation
  of the allocation/deallocation procedures according to the following scheme:
  -1: it occurs at return value;
   0: it occurs at the 1st parameter
   1: it occurs at the 2nd parameter
   ...
   n: it occurs at the (n+1)th parameter

   EXAMPLE:
   Find_Missing_Resource_Release("malloc", -1, "free", 0) looks for
   resources allocated by malloc (which are given by its return value, hence the
   -1 argument) but not released by free (which are given by its first parameter,
   hence the 0 argument.)
*/


if (param.Length == 4)
{
	// Fetch query options
	string openResourceFunctionName = (string) param[0];
	int openResourceIndex = (int) param[1];
	string closeResourceFunctionName = (string) param[2];
	int closeResourceIndex = (int) param[3];
	
	// Hereafter the following conditions are assumed to be met.
	// (0)   openResourceIndex >= -1
	// (1)   closeResourceIndex >= 0
	
	CxList methods = Find_Methods();
	CxList openResourceFunctionOccurrences = methods.FindByShortName(openResourceFunctionName);
	CxList closeResourceFunctionOccurrences = methods.FindByShortName(closeResourceFunctionName);
	
	CxList openResources = All.NewCxList();
	if (openResourceIndex > -1)
	{
		// The variable holding the resource is being passed as an argument to
		// the resource allocating procedure. In this case we simply extract the
		// corresponding argument.
		openResources = Find_UnknownReference().GetParameters(openResourceFunctionOccurrences, openResourceIndex);
	}
	else
	{
		// The variable holding the resource is being returned from the resource
		// allocating procedure. In this case we extract the left hand-side (LHS)
		// of the top assignment expression, if it exists.
		
		openResources = openResourceFunctionOccurrences.FindByType(typeof(MethodInvokeExpr));
		openResources = openResources.FindByType(typeof(AssignExpr)).FindByAssignmentSide(CxList.AssignmentSide.Left);
	}
	
	CxList closeResources = All.GetParameters(closeResourceFunctionOccurrences, closeResourceIndex);
	result = All.FindDefinition(openResources) - All.FindDefinition(closeResources);
}