/*
MISRA C RULE 14-9
------------------------------
This query searches for usage of if constructs that are not followed by a compound statements, or 
else keyword that are not followed by either a compound statement or another if statement.

	The Example below shows code with vulnerability: 

if (i>5)
	i=10;

*/

//find all if-else statements
CxList ifs = All.FindByType(typeof(IfStmt));
CxList elseStatements = All.NewCxList();

// find else statements with no statement collection and are not ifelse statements
foreach(CxList cur in ifs){
	try{
		StatementCollection falseStmts = (cur.data.GetByIndex(0) as IfStmt).FalseStatements;
		if (falseStmts != null && falseStmts.Count == 1){
			CxList tempNode = All.FindById(falseStmts[0].NodeId);
			if (falseStmts.Father.ToString().Equals("N/A") && tempNode.FindByType(typeof(IfStmt)).Count == 0 )
			{
				elseStatements.Add(tempNode);
			}
		}
	}
	catch (Exception e){
		cxLog.WriteDebugMessage(e);
	}
}


// remove ifs with a non empty compound statements
ifs -= All.FindByType(typeof(StatementCollection)).GetFathers();

// remove ifs with an empty compound statement
foreach(CxList cur in ifs){
	try{
		StatementCollection trueStmts = (cur.data.GetByIndex(0) as IfStmt).TrueStatements;
		if (trueStmts != null && trueStmts.Count == 0){
			ifs -= cur;
		}
	}
	catch (Exception e)
	{
		cxLog.WriteDebugMessage(e);
	}
}

result = ifs + elseStatements;