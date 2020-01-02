//	Arithmetic Operation On Boolean
//  -------------------------------
//  Find all the arithmetic operations on boolean, excluding boolean
//  operations (or, and, equality, not)
///////////////////////////////////////////////////////////////////////

// Find all booleans
CxList bools = Find_Builtin_Types().FindByType("bool");
bools.Add(Find_BinaryExpressions().FindByShortNames(new List<string>(new string[] {"<",">","||","&&","=="}), false));

//Add assign expressions that there left side is boolean
CxList assignExprs=Find_AssignExpr();
bools.Add((All.FindByFathers(assignExprs).FindByAssignmentSide(CxList.AssignmentSide.Left) * bools).GetFathers());

//Find ternary expressions that returns bool
CxList expressions = Find_TernaryExpr();
foreach(CxList expr in expressions){
	CxList sons = All.FindByFathers(expr);	
	if(sons.Count == (sons * bools).Count)
		bools.Add(expr);
}

// Look for unauthorized unary operations
CxList unary = bools.GetFathers().FindByType(typeof(UnaryExpr));
unary -= unary.FindByShortName("Not");
unary -= unary.FindByShortName("Address");
unary -= unary.FindByShortName("Pointer");

// Look for unauthorized binary operations
CxList binary = bools.GetFathers().FindByType(typeof(BinaryExpr));
binary -= binary.FindByShortName("||");
binary -= binary.FindByShortName("&&");
binary -= binary.FindByShortName("==");
binary -= binary.FindByShortName("!=");
binary -= binary.FindByShortName("|");


// Remove also Shift right and left,bitwise-and and bitwise-xor (I/O) - that have no short name
foreach (CxList bin in binary)
{
	BinaryExpr b = bin.data.GetByIndex(0) as BinaryExpr;
	if ((b.Operator == BinaryOperator.ShiftRight)
		|| (b.Operator == BinaryOperator.ShiftLeft) || (b.Operator == BinaryOperator.BitwiseAnd) || (b.Operator == BinaryOperator.BitwiseXor))
	{
		binary -= bin;
	}
}

// Return the unauthorized operations
result = binary;
result.Add(unary);