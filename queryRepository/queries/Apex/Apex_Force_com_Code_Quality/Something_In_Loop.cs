/*
This query checks if any of the values in an input CxList appears in any type of loop.
We do 2 different checks - one for typeof(IterationStmt) and one for typeof(ForEachStmt)

Parameters:
	param[0] - the input CxList, which we check if it contains members that are in a loop
	param[1] - a relevant set of objects in which we test this query
	param[2] - a flag, ... have to check :)
*/
if (param.Length == 3)
{
	CxList listToCheck = param[0] as CxList;
	CxList subset = param[1] as CxList;
	bool checkForNull = (bool) param[2];
	
	CxList bareListToCheck = listToCheck;	// ??
	
	// Find apex code
	CxList apex = Find_Apex_Files();
	
	// Calculate all methods calls that contain a member of the listToCheck, up to 5 level deep
	CxList methods = apex.FindAllReferences(listToCheck.GetAncOfType(typeof(MethodDecl)));
	int numMeth = 0;
	for(int i = 0; i < 5 && methods.Count > numMeth; i++)
	{
		numMeth = methods.Count;
		methods.Add(apex.FindAllReferences(methods.GetAncOfType(typeof(MethodDecl))));
	}

	// Add these methods to the list to check, because these methods might also be called in a loop
	listToCheck.Add(methods);
	
	// Leave only results in the defined subset
	listToCheck *= subset;

	/// Part 1 - IterationStmt
	CxList iterations = listToCheck.GetAncOfType(typeof(IterationStmt));
	CxList listToCheckIter = listToCheck.GetByAncs(iterations);
	CxList somethingInIteration = All.NewCxList();
	foreach (CxList iteration in iterations)
	{
		try
		{
			IterationStmt iter = iteration.data.GetByIndex(0) as IterationStmt;
			if (iter != null)
			{
				CxList memberInLoop = listToCheckIter.GetByAncs(iteration);

				if (memberInLoop.Count > 0)
				{
					somethingInIteration.Add(iteration.Concatenate(Connect_Loop_To_DB(memberInLoop, bareListToCheck, checkForNull), true));
				}
			}
		}
		catch (Exception ex)
		{
			cxLog.WriteDebugMessage(ex);
		}
	}

	/// Part 2 - ForEachStmt

	// Get things inside a foreach statement
	CxList forEach = listToCheck.GetAncOfType(typeof(ForEachStmt));
	
	// Remove listToCheck values that are the collection of the FOR loop
	foreach (CxList loop in forEach)
	{
		try
		{
			ForEachStmt l = loop.data.GetByIndex(0) as ForEachStmt;
			CxList statementsToRemove = All.FindById(l.Statements[0].NodeId) + All.FindById(l.Collection.NodeId);
			listToCheck -= listToCheck.GetByAncs(statementsToRemove);
		}
		catch (Exception ex)
		{
			cxLog.WriteDebugMessage(ex);
		}
	}

	CxList listToCheckInForEach = listToCheck.GetByAncs(forEach);

	CxList somethingInForEach = All.NewCxList();
	CxList inForEach = All.GetByAncs(forEach);
	foreach (CxList iteration in forEach)
	{
		try
		{

			CxList memberInLoop = listToCheckInForEach.GetByAncs(iteration);
			if (memberInLoop.Count > 0)
			{
				ForEachStmt l = iteration.data.GetByIndex(0) as ForEachStmt;
				CxList firstStatement = All.FindById(l.Statements[0].NodeId);
				CxList start = All.FindById(l.Collection.NodeId) + inForEach.GetByAncs(firstStatement);
				start -= start.DataInfluencingOn(start);
				CxList addition = Connect_Loop_To_DB(memberInLoop, bareListToCheck, checkForNull).DataInfluencedBy(start);
				if (addition.Count == 0)
				{
					addition = iteration.Concatenate(Connect_Loop_To_DB(memberInLoop, bareListToCheck, checkForNull));
				}
				somethingInForEach.Add(addition);
			}
		}
		catch (Exception ex)
		{
			cxLog.WriteDebugMessage(ex);
		}
	}

	result = somethingInIteration + somethingInForEach;
	result -= Find_Test_Code();
}