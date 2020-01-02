CxList returnStatements = Find_ReturnStmt();

CxList assignLeftRef = Find_AssignNotInfluencingAnthingLeft();
CxList assignRef = Find_AssignRefNotInfluencing();


CxList doubleAssign = assignLeftRef - assignLeftRef.DataInfluencingOn(assignRef);

// Remove double assign in conditions for cases like while(line=doSomething()), where the "line" actually
// influences nothing (flow issue), although it is used later on
doubleAssign -= doubleAssign.GetByAncs(Find_Conditions());

// Removes variables from assigns within return statements, for example: return variable=5;
doubleAssign -= doubleAssign.GetByAncs(returnStatements);

// remove cases when there are various initializations in different if blocks

CxList ifStmts = doubleAssign.GetAncOfType(typeof(IfStmt));
CxList toRemove = All.NewCxList();
foreach (CxList inIf in ifStmts)
{
	CxList dblInIf = doubleAssign.GetByAncs(inIf);
	CxList inTrue = inIf.GetBlocksOfIfStatements(true);
	CxList dblInTrue = dblInIf.GetByAncs(inTrue);
	if ((dblInTrue.Count > 0) && (dblInTrue.Count < dblInIf.Count))
	{
		toRemove.Add(dblInIf);
	}
}
result = doubleAssign - toRemove;