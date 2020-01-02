/*
	This query finds Path Traversal occurrences. It works like this:
 	- Finds requests that might carry paths
 	- Finds places where these paths are used to access info
	- Finds outputs to where the previous info can be sent
*/
CxList inputs = All.NewCxList();
CxList fileAccesses = All.NewCxList();
CxList outputs = All.NewCxList();
CxList allSanitizers = All.NewCxList();

// Get all possible inputs that can make a Path Traversal
inputs.Add(Find_Inputs());

// Find inputs that comes from previouly data inserted by the user to the DB
inputs.Add(Find_DB_Out());

// The places where unwanted actions takes place ( Read or Write Files )
fileAccesses.Add(Find_File_Write());
fileAccesses.Add(Find_File_Read());

// Get all possible SANITIZERS that prevente the path traversal
CxList dotdots = All.NewCxList();
string[] possibleValidReplacements = {"../","..\\\\",".."}; // sentences used for path traversal

foreach(String r in possibleValidReplacements)
{
	IAbstractValue absValue = new StringAbstractValue(r);
	dotdots.Add(All.FindByAbstractValue(abstractValue => abstractValue.IncludedIn(absValue)));
}

CxList parameters = All.NewCxList();
CxList validReplaceParams = All.NewCxList();
CxList methodsWithSanitizedParams = All.NewCxList();

parameters.Add(All.GetParameters(All.FindByMemberAccess("\"strings\".Replace"), 1));
parameters.Add(All.GetParameters(All.FindByMemberAccess("\"strings\".NewReplacer"), 0));

validReplaceParams.Add(dotdots * parameters);
methodsWithSanitizedParams.Add(validReplaceParams.GetAncOfType(typeof(MethodInvokeExpr)));
allSanitizers.Add(methodsWithSanitizedParams);

// Here we need to consider all instances of NewReplacer
CxList newReplacerAss = All.NewCxList();
CxList newReplacerRef = All.NewCxList();
CxList newReplacerSan = All.NewCxList();
newReplacerAss.Add(methodsWithSanitizedParams.FindByShortName("NewReplacer").GetAssignee());
newReplacerRef.Add(All.FindAllReferences(newReplacerAss));
newReplacerSan.Add(newReplacerRef.GetMembersOfTarget().FindByShortName("Replace"));
allSanitizers.Add(newReplacerSan);

CxList isAbs = All.FindByMemberAccess("\"path\".IsAbs");
CxList isNotAbs = isAbs.GetFathers().FindByType(typeof(UnaryExpr)).FindByShortName("Not");

// If Statments that includes isAbs function
CxList ifsWithIsAbsStmt = isAbs.GetAncOfType(typeof(IfStmt));
CxList ifsWithIsNotAbsStmt = isNotAbs.GetAncOfType(typeof(IfStmt));
ifsWithIsAbsStmt = ifsWithIsAbsStmt - ifsWithIsNotAbsStmt;

// All IF blocks containing the sanitizers
CxList ifBlocksWithIsAbs = allSanitizers.GetByAncs(ifsWithIsAbsStmt);
CxList ifBlocksWithIsNotAbs = allSanitizers.GetByAncs(ifsWithIsNotAbsStmt);

// Reduce sanitizers scope to places where the input path is relative and not absolute
CxList ifsBlocksSanitized = All.NewCxList();
CxList fBlocks = All.NewCxList();
CxList tBlocks = All.NewCxList();
fBlocks.Add(All.GetBlocksOfIfStatements(false));
tBlocks.Add(All.GetBlocksOfIfStatements(true));
ifsBlocksSanitized.Add(ifBlocksWithIsAbs.GetByAncs(fBlocks));
ifsBlocksSanitized.Add(ifBlocksWithIsNotAbs.GetByAncs(tBlocks));

// Flow
result = inputs.InfluencingOnAndNotSanitized(fileAccesses, ifsBlocksSanitized).ReduceFlowByPragma();