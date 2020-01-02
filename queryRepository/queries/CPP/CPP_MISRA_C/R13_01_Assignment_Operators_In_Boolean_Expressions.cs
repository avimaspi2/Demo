/*
MISRA C RULE 13-1
------------------------------
This query searches for assignments inside boolean expressions

	The Example below shows code with vulnerability: 

int a;
if (a=5)
	return 0;

int b = !(a=6);

*/

// this catches boolean expressions that are operands
CxList Exprs = All.FindByType(typeof(BinaryExpr)) + All.FindByType(typeof(UnaryExpr));

// only choose boolean expressions
Exprs = Exprs.FindByName("==") +
	Exprs.FindByName("!=") +
	Exprs.FindByName("Not") + 
	Exprs.FindByName("<") +
	Exprs.FindByName("<=") +
	Exprs.FindByName(">") +
	Exprs.FindByName(">=") +
	Exprs.FindByName("&&") +
	Exprs.FindByName("||") +
	Exprs.FindByName("&") +
	Exprs.FindByName("|") +
	Exprs.FindByName("^");


	CxList potentials = All.FindByFathers(Exprs);

	// now add conditions in control structures
	CxList Iterations = All.FindByType(typeof(IterationStmt));
	CxList Ifs = All.FindByType(typeof(IfStmt));
	CxList sons = All.FindByFathers(Ifs) + All.FindByFathers(Iterations);


foreach (CxList cur in Ifs){
	IfStmt curIf = cur.data.GetByIndex(0) as IfStmt;
	if(curIf != null)
	{
		CSharpGraph curExpr = ((CSharpGraph) curIf.Condition);
		if(curExpr != null)
		{
			potentials.Add(sons.FindById(curExpr.NodeId));
		}
	}
		
}

foreach (CxList cur in Iterations){
	
	IterationStmt curIter = cur.data.GetByIndex(0) as IterationStmt;

	if(curIter != null)
	{
		CSharpGraph curExpr = ((CSharpGraph) curIter.Test);
		if (curExpr != null) 
		{
			potentials.Add(sons.FindById(curExpr.NodeId));
		}
	}
}


	result.Add(potentials.FindByType(typeof(AssignExpr)));