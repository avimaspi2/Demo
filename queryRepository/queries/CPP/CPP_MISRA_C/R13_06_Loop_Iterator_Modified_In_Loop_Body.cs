/*
MISRA C RULE 13-6
------------------------------
This query searches for loop iterators changed within the body of the loop

	The Example below shows code with vulnerability: 

for ( i = 0; i < f; i++ ){
	if (i==3)
		i=5;
}

*/


// find all for loops
CxList forIterations = All.FindByType(typeof(IterationStmt));
foreach (CxList cur in forIterations){
	IterationStmt curIter = (IterationStmt) cur.data.GetByIndex(0);
	if(curIter != null && curIter.IterationType != null)
	{
		if (curIter.IterationType != IterationType.For){
			forIterations -= cur;
		}
	}
}
CxList forIterators = All.NewCxList();
CxList unknownRefs = All.FindByType(typeof(UnknownReference));
CxList allForsDescendants = All.GetByAncs(forIterations);

// remove boolean instances
// first find and remove instances of a type that is typedefd bool
CxList typedefBools = All.FindByType(typeof(StringLiteral)).FindByName("CX_TYPEDEF").GetFathers().FindByShortName("*bool*");
ArrayList boolTypes = new ArrayList();

foreach(CxList cur in typedefBools){
		
	string typeName = ((Declarator) cur.data.GetByIndex(0)).Name;
	if(typeName != null)
	{
		if (!boolTypes.Contains(typeName)){
			boolTypes.Add(typeName);
			boolTypes.Add("*." + typeName);
		}
	}
}
CxList boolDecls = Find_All_Declarators().FindByTypes((string[]) boolTypes.ToArray(typeof(string)));
unknownRefs -= unknownRefs.FindAllReferences(boolDecls);

// now find operators that receive boolean, and remove their direct inputs
CxList binaryExprs = All.FindByType(typeof(BinaryExpr));
CxList unaryExprs = All.FindByType(typeof(UnaryExpr));
CxList recBoolean = binaryExprs.GetByBinaryOperator(BinaryOperator.BooleanOr) +
	binaryExprs.GetByBinaryOperator(BinaryOperator.BooleanAnd);
foreach (CxList cur in unaryExprs){
	UnaryOperator curOp = ((UnaryExpr) cur.data.GetByIndex(0)).Operator;
	if(curOp != null)
	{
		if (curOp == UnaryOperator.Not){
			recBoolean.Add(cur);
		}
	}
}

unknownRefs -= unknownRefs.FindByFathers(recBoolean);

CxList preIncDec = All.NewCxList();

// add prefix increment and decrement to the checked assignments
foreach (CxList cur in unaryExprs){
	UnaryOperator curOp = ((UnaryExpr) cur.data.GetByIndex(0)).Operator;
	if(curOp != null)
	{
		if ((curOp == UnaryOperator.Increment) || (curOp == UnaryOperator.Decrement)){
			preIncDec.Add(All.FindById(((UnaryExpr) cur.data.GetByIndex(0)).Right.NodeId));
		}
	}
}

// add postfix increment and decrement to the checked assignments
CxList postfixExprs = All.FindByType(typeof(PostfixExpr));
CxList postIncDec = All.NewCxList();
foreach (CxList cur in postfixExprs){

	PostfixOperator curOp = ((PostfixExpr) cur.data.GetByIndex(0)).Operator;
	if(curOp != null)
	{
		if ((curOp == PostfixOperator.Increment) || (curOp == PostfixOperator.Decrement)){
			if (((PostfixExpr) cur.data.GetByIndex(0)).Left != null){
				postIncDec.Add(All.FindById(((PostfixExpr) cur.data.GetByIndex(0)).Left.NodeId));
			}
		}
	}
}

// now go over for loop and check assignments
foreach (CxList cur in forIterations){

	CxList body = All.NewCxList();
	CxList inits = All.NewCxList();
	CxList incs = All.NewCxList();
	IterationStmt curIteration = (IterationStmt) cur.data.GetByIndex(0);
	if(curIteration != null)
	{		
		StatementCollection bodyCol = curIteration.Statements;
		StatementCollection initCol = curIteration.Init;
		StatementCollection incCol = curIteration.Increment;
		CxList test = All.NewCxList();
		Expression t = curIteration.Test;
		if(t != null)
		{
			test = All.FindById(t.NodeId);		
		}
		
		// build the set of current iteration iterators
		foreach(Statement bodyPart in bodyCol){
			if(bodyPart != null)
			{
				body.Add(All.FindById(bodyPart.NodeId));
			}
		}
			
		foreach(Statement init in initCol){
			if(init != null)
			{
				inits.Add(All.FindById(init.NodeId));
			}
		}
		foreach(Statement inc in incCol){
			if(inc != null)
			{
				incs.Add(All.FindById(inc.NodeId));
			}
		}
			
		inits = unknownRefs.GetByAncs(inits);
		incs = unknownRefs.GetByAncs(incs);
		if(test != null)
		{
			test = unknownRefs.GetByAncs(test);
		}
		body = allForsDescendants.GetByAncs(body);
		
		// more accurate representation of the iterators
		CxList curIterators = incs.FindByName(test).FindByName(inits);

		CxList bodyRefs = body.FindAllReferences(curIterators);
		// direct assigments
		result.Add(bodyRefs.FindByAssignmentSide(CxList.AssignmentSide.Left));
		// postfix/prefix increment/decrement
		result.Add(bodyRefs * (preIncDec + postIncDec));
		// passed by ref to function
		CxList adressObjs = body.FindByType(typeof(UnaryExpr)).FindByShortName("Address");
		result.Add(bodyRefs.FindByFathers(adressObjs.FindByFathers(body.FindByType(typeof(Param)))));
	
	}

}