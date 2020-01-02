/*

This query will look for a method calling itself, when there is no "return" statement inside a loop
that is not affected by the recursive call.

The query is a bit of heuristic.

Before passing on all the methods to see if they are recursive (for which we need a loop on the methods)
we only keep the methods with that call other methods, and do not contain the above return statement. This
is important for performance issues, because this way we have a shorter list of potential methods..

If recursive call present in if , while, foreach and ternary we decide that it is not uncontrolled recursion

*/

// Get only methods that are defined and used
CxList methods = Find_Methods();
methods = methods.FindAllReferences(All.FindDefinition(methods));

// Find all methods that contain other methods

CxList methodOfMethod = All.GetMethod(methods);

// If statement in these methods.
CxList IfStmt = Find_Ifs();
IfStmt = IfStmt.GetByAncs(methodOfMethod);

CxList ForEachStmt = Find_ForEachStmt();
CxList iterationStmt = Find_IterationStmt();
CxList TernaryExpr = Find_TernaryExpr();

CxList conditionStatement = All.NewCxList();
conditionStatement.Add(ForEachStmt);
conditionStatement.Add(iterationStmt);
conditionStatement.Add(TernaryExpr);
conditionStatement = conditionStatement.GetByAncs(methodOfMethod);
conditionStatement.Add(IfStmt); 

// A return statement inside the above "if" statements, that is not affected by a recursive method call
CxList ret = Find_ReturnStmt();
ret = ret.GetByAncs(IfStmt);
ret -= methods.GetByAncs(ret).GetAncOfType(typeof(ReturnStmt));

// Leave only relevant methods
methodOfMethod -= methodOfMethod.GetMethod(ret);

CxList CxInternalMethods = methodOfMethod.FindByShortName("Checkmarx_Class_Init*");
methodOfMethod -= CxInternalMethods;

// Loop on all remaining methods and leave only the ones calling themselves (=> recursive)
foreach (CxList method in methodOfMethod)
{
	// The actual use of recursion
	CxList useRecursions = methods.FindAllReferences(method).GetByAncs(method);
	// The method that calls the recursion
	CxList methodDecl = methodOfMethod.GetMethod(useRecursions);
	// If there exists such a method/call 
	if (methodDecl.Count > 0)
	{
		foreach (CxList useRecursion in useRecursions)
		{
			// test if it inside of condition statement -->if not add it to the result
			if (useRecursion.GetByAncs(conditionStatement).Count == 0)
			{
				result.Add(methodDecl.ConcatenateAllTargets(useRecursion));
			}
		}
	}
}