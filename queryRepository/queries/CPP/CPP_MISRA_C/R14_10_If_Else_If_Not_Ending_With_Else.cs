/*
MISRA C RULE 14-10
------------------------------
This query searches for if else if structures not ending with else

	The Example below shows code with vulnerability: 

if (i > 0)
	{
		...
	}
	else if (i = 0){
		 ...
	}		

*/

CxList ifs = All.FindByType(typeof(IfStmt));
CxList elseIfs = All.NewCxList();

// find else if statements
foreach(CxList cur in ifs){
	StatementCollection falseStmts = (cur.data.GetByIndex(0) as IfStmt).FalseStatements;
	if (falseStmts != null && falseStmts.Count == 1){
		IfStmt elseIf = falseStmts[0] as IfStmt;
		if (elseIf != null){
			elseIfs.Add(All.FindById(elseIf.NodeId));
		}
	}
}
elseIfs -= elseIfs.FindByFathers(All.FindByType(typeof(StatementCollection)));

// remove else ifs that have an else with a non comment statement
foreach(CxList cur in elseIfs){
	StatementCollection falseStmts = (cur.data.GetByIndex(0) as IfStmt).FalseStatements;
	if (falseStmts != null && falseStmts.Count != 0){
		elseIfs -= cur;
	}
}

// remove else ifs that have an else with a comment statement
elseIfs -= elseIfs.FindByRegex(@"[\W]if[^;\{]*{[^\}]*}[(\s)(\r\n)]*else[(\s)(\r\n)]*{[^\}]*?[(/\*)(//)]");

result = elseIfs;