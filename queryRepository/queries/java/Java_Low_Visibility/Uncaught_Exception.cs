CxList catchStmt = base.Find_Catch();
// Find (only) the try statements that have a catch statement
CxList tryStmt = catchStmt.GetFathers();
CxList allMethodsRef = Find_Methods();
CxList allMethodsDecl = Find_MethodDeclaration();

CxList throwStmt = Find_ThrowStmt();
throwStmt.Add(Find_Optional_orElseThrow());

// Only throws that are not under try-catch
CxList potentialThrows = throwStmt - throwStmt.GetByAncs(tryStmt);

// pass on all the throws, and see if any of them is problematic
foreach (CxList pt in potentialThrows)
{
	// Find all the methods that contain the problematic throw
	CxList methodDecl = allMethodsDecl.GetMethod(pt);
	// If it's the "main" method, it is a vulnerability
	if (methodDecl.FindByShortName("main").Count > 0)
	{
		result.Add(pt.ConcatenateAllTargets(methodDecl));
	}
	else
	{
		// All the references of these methods
		CxList methodReferences = allMethodsRef.FindAllReferences(methodDecl) - methodDecl;
		// Remove the references under a try block with catch
		methodReferences -= methodReferences.GetByAncs(tryStmt);
		int loopCounter = 0; // limit the while, to be ready for an infinite loop bug
		// Look recursively on all calls to the method that contains a throw statement
		while (++loopCounter < 5 && methodReferences.Count > 0)
		{
			methodDecl = allMethodsDecl.GetMethod(methodReferences);
			methodReferences = allMethodsRef.FindAllReferences(methodDecl) - methodDecl;
			CxList mainMethod = methodDecl.FindByShortName("main");
			if (mainMethod.Count > 0)
			{
				result.Add(pt.ConcatenateAllTargets(mainMethod));
				break;
			}
			methodReferences -= methodReferences.GetByAncs(tryStmt);
		}
	}
}