// Get all declarators, param decl and constant decl
CxList declarators = Find_Declarators();

// declarators that are initialized by somehting
CxList initializer = All.GetByAncs(declarators).FindByAssignmentSide(CxList.AssignmentSide.Right);
initializer = initializer.GetAncOfType(typeof(Declarator));

// the references of all initialized members
CxList initializedRef = All.FindAllReferences(initializer);
initializedRef -= Find_Dead_Code_Contents();
// Initialized variables that are re-initialized in the program
CxList reInitialized = (initializedRef - initializer).FindByAssignmentSide(CxList.AssignmentSide.Left);

CxList removeReInit = All.NewCxList();
foreach (CxList reInit in reInitialized)
{
	CxList IfStmt = reInit.GetAncOfType(typeof(IfStmt));
	CxList blk = reInit.GetAncOfType(typeof(StatementCollection));
	//if (reInitialized.GetByAncs(blk).Count < reInitialized.GetByAncs(IfStmt).Count)
	if (blk.GetByAncs(IfStmt).Count > 0)
	{
		removeReInit.data.AddRange(reInit.data);
	}
}
reInitialized -= removeReInit;

// Find all the references of these re-initialized variables
CxList reInitializedRef = initializedRef.FindAllReferences(reInitialized);

// Look for uses as parameters of undefined functions.
// For example insert(x) is affected by x
CxList methodsOfParams = Find_Methods().FindByParameters(initializedRef);
methodsOfParams -= methodsOfParams.FindAllReferences(All.FindDefinition(methodsOfParams));
CxList usedAsParams = initializedRef.GetByAncs(methodsOfParams);

// There are cases where the initialized variable is used in one place, and then changed and reused, so
// we have to remove these
CxList initUsedAsParams = All.NewCxList();
initUsedAsParams.Add(initializer);
initUsedAsParams.Add(usedAsParams);

CxList usedWithoutReinitialization = reInitializedRef.DataInfluencedBy(initUsedAsParams);
usedWithoutReinitialization = initializedRef.FindAllReferences(usedWithoutReinitialization);

// The right side of the reinitialized variables
CxList reInitializedRightSide = All.GetByAncs(reInitialized.GetAncOfType(typeof(AssignExpr))).FindByAssignmentSide(CxList.AssignmentSide.Right);

// All reinitialized variables, that are then used, but never used by the original initializedValue

CxList allInitRefs = All.NewCxList();
allInitRefs.Add(reInitializedRef);
allInitRefs -= usedWithoutReinitialization;
allInitRefs -= reInitialized;	

CxList reinitializedAndUsed = reInitializedRightSide.DataInfluencingOn(allInitRefs);
reinitializedAndUsed = reInitialized.GetByAncs(reinitializedAndUsed.GetAncOfType(typeof(AssignExpr)));
reinitializedAndUsed = reInitialized.FindAllReferences(reinitializedAndUsed);

// Remove cases in tr-catch, because it's a standard to get an initial value in case the try-catch fails
reinitializedAndUsed -= reinitializedAndUsed.GetByAncs(reinitializedAndUsed.GetAncOfType(typeof(TryCatchFinallyStmt)));

// Remove Array Indexes
reinitializedAndUsed -= reinitializedAndUsed.FindByType(typeof(IndexerRef));

result = reinitializedAndUsed;