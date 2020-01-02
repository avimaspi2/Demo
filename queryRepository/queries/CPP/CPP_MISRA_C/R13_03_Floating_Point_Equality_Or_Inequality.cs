/*
MISRA C RULE 13-3
------------------------------
This query searches for  floating point operands checked for equality or inequality directly (==, !=) or indirectly (<= && >=)

	The Example below shows code with vulnerability: 

float f = 0.1234f;
float g = 0.1234f;
    
if ( f == g ){
	c = 1;   
}

if ( f <= g && f >= g ){
	c = 1;
}

*/

// first we build a list of all floating point typedefs
CxList typedefDecls = All.FindByType(typeof(StringLiteral)).FindByName("CX_TYPEDEF").GetFathers();
CxList floatTypedefDecls = typedefDecls.FindByType("float") + typedefDecls.FindByType("double");
ArrayList floatTypes = new ArrayList();
floatTypes.Add("float");
floatTypes.Add("double");
foreach(CxList cur in floatTypedefDecls){
	Declarator g = (Declarator) cur.data.GetByIndex(0);
	if(g == null || g.Name == null) {
		continue;
	}
	string typeName = g.Name;
	if (!floatTypes.Contains(typeName)){
		floatTypes.Add(typeName);
		floatTypes.Add("*." + typeName);
	}
}

// now build a list of all floating point declarators
CxList floatDecls = Find_All_Declarators().FindByTypes((string[]) floatTypes.ToArray(typeof(string)));
floatDecls -= typedefDecls;

// uses are decl instances
CxList fpoints = All.FindAllReferences(floatDecls).FindByType(typeof(UnknownReference)) +
	All.FindByType(typeof(RealLiteral));
// remove all casted
CxList castIntoTypes = All.FindByFathers(All.FindByType(typeof(CastExpr))).FindByType(typeof(TypeRef));
fpoints -= fpoints.GetByAncs(castIntoTypes);
foreach(CxList cur in castIntoTypes){
	TypeRef g = (TypeRef) cur.data.GetByIndex(0);
	if(g == null || g.FullName == null) {
		continue;
	}
	string curName = g.FullName;
	// if irrelevant type, remove it from casted list
	if (!floatTypes.Contains(curName)){
		castIntoTypes -= cur;
	}	
}
// add back relevant casted
fpoints.Add(All.GetByAncs(castIntoTypes.GetFathers()).FindByType(typeof(UnknownReference)) +
	All.GetByAncs(castIntoTypes.GetFathers()).FindByType(typeof(RealLiteral)));

// now that we have the floating point objects, add direct comparisons to results
CxList directComparisons = All.FindByType(typeof(BinaryExpr)).FindByName("==") + 
	All.FindByType(typeof(BinaryExpr)).FindByName("!=");
result.Add(fpoints.FindByFathers(directComparisons));

// for indirect comparisons, first find the relevant comparison and logical expressions
CxList lte = fpoints.GetFathers().GetByBinaryOperator(BinaryOperator.LessThanOrEqual);
CxList gte = fpoints.GetFathers().GetByBinaryOperator(BinaryOperator.GreaterThanOrEqual);
CxList diffWeak = lte + gte;
CxList lt = fpoints.GetFathers().GetByBinaryOperator(BinaryOperator.LessThan);
CxList gt = fpoints.GetFathers().GetByBinaryOperator(BinaryOperator.GreaterThan);
CxList diffStrong = lt + gt;
CxList allDiffs = diffWeak + diffStrong;

// logical conditions
CxList andExpr = allDiffs.GetFathers().GetByBinaryOperator(BinaryOperator.BooleanAnd);
CxList orExpr = allDiffs.GetFathers().GetByBinaryOperator(BinaryOperator.BooleanOr);

// iteration optimization
CxList andOrExprSons = All.FindByFathers(andExpr + orExpr);
CxList fpointOperands = fpoints.FindByFathers(allDiffs);

// test indirect "==" such as (x>=y && x<=y)
foreach (CxList cur in andExpr){
	CxList curSons = andOrExprSons.FindByFathers(cur);
	
	// currently not supporting logical conditions with more than two sons
	if ((curSons.Count != 2) || ((curSons * diffWeak).Count != 2)){
		continue;
	}	
	
	// at this point we know only there are exactly two sons, both are <= or >=
	BinaryExpr firstOp = curSons.data.GetByIndex(0) as BinaryExpr;
	BinaryExpr secondOp = curSons.data.GetByIndex(1) as BinaryExpr;
	if(firstOp == null || secondOp == null ||
		firstOp.Left == null || secondOp.Left == null ||
		firstOp.Right == null || secondOp.Right == null) {
		continue;
	}
	CxList firstLeft = fpointOperands.FindById(((CSharpGraph) firstOp.Left).NodeId);
	CxList firstRight = fpointOperands.FindById(((CSharpGraph) firstOp.Right).NodeId);
	CxList SecondLeft = fpointOperands.FindById(((CSharpGraph) secondOp.Left).NodeId);
	CxList Secondright = fpointOperands.FindById(((CSharpGraph) secondOp.Right).NodeId);
	bool FLeqSL = (firstLeft.FindByName(SecondLeft).Count != 0);
	bool FLeqSR = (firstLeft.FindByName(Secondright).Count != 0);
	bool FReqSL = (firstRight.FindByName(SecondLeft).Count != 0);
	bool FReqSR = (firstRight.FindByName(Secondright).Count != 0);
	// (x>=y && y>=x), (x<=y && y<=x)
	if ((FLeqSR && FReqSL) && (firstOp.Operator == secondOp.Operator)){
		result.Add(fpointOperands.FindByFathers(curSons));
	}
		// (x>=y && x<=y), (x<=y && x>=y)	
	else if ((FLeqSL && FReqSR) && (firstOp.Operator != secondOp.Operator)){
		result.Add(fpointOperands.FindByFathers(curSons));
	}
}
// test indirect "!=" such as (x>y || x<y)
foreach (CxList cur in orExpr){
	CxList curSons = andOrExprSons.FindByFathers(cur);
	
	// currently not supporting logical conditions with more than two sons
	if ((curSons.Count != 2) || ((curSons * diffStrong).Count != 2)){
		continue;
	}	
	
	// at this point we know only there are exactly two sons, both are < or >
	BinaryExpr firstOp = curSons.data.GetByIndex(0) as BinaryExpr;
	BinaryExpr secondOp = curSons.data.GetByIndex(1) as BinaryExpr;
	if(firstOp == null || secondOp == null ||
		firstOp.Left == null || secondOp.Left == null ||
	firstOp.Right == null || secondOp.Right == null) {
		continue;
	}
	CxList firstLeft = fpointOperands.FindById(((CSharpGraph) firstOp.Left).NodeId);
	CxList firstRight = fpointOperands.FindById(((CSharpGraph) firstOp.Right).NodeId);
	CxList SecondLeft = fpointOperands.FindById(((CSharpGraph) secondOp.Left).NodeId);
	CxList Secondright = fpointOperands.FindById(((CSharpGraph) secondOp.Right).NodeId);
	bool FLeqSL = (firstLeft.FindByName(SecondLeft).Count != 0);
	bool FLeqSR = (firstLeft.FindByName(Secondright).Count != 0);
	bool FReqSL = (firstRight.FindByName(SecondLeft).Count != 0);
	bool FReqSR = (firstRight.FindByName(Secondright).Count != 0);
	// (x>y || y>x), (x<y || y<x)
	if ((FLeqSR && FReqSL) && (firstOp.Operator == secondOp.Operator)){
		result.Add(fpointOperands.FindByFathers(curSons));
	}
		// (x>y || x<y), (x<y || x>y)	
	else if ((FLeqSL && FReqSR) && (firstOp.Operator != secondOp.Operator)){
		result.Add(fpointOperands.FindByFathers(curSons));
	}
}