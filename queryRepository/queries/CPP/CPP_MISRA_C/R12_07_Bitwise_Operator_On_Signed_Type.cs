/*
MISRA C RULE 12-7
------------------------------
This query searches for bitwise operators (~, <<, <<=, >>, >>=, &, &=, ^, ^=, |, |=) 
used on an operand with underlying signed type 

	The Example below shows code with vulnerability: 

int16_t mc2_1207_a;
int16_t mc2_1207_b;
int16_t mc2_1207_c;

mc2_1207_c = ~mc2_1207_a;
mc2_1207_c = mc2_1207_a << mc2_1207_b;     

*/

// first we build a list of all signed typedefs
CxList typedefDecls = All.FindByType(typeof(StringLiteral)).FindByName("CX_TYPEDEF").GetFathers();
CxList typedefSgnDecls = typedefDecls.FindByExtendedType("signed");
ArrayList sgnTypes = new ArrayList();
foreach(CxList cur in typedefSgnDecls){
	Declarator g = (Declarator) cur.data.GetByIndex(0);
	if (g == null || g.Name == null) {
		continue;
	}
	string typeName = g.Name;
	if (!sgnTypes.Contains(typeName)){
		sgnTypes.Add(typeName);
		sgnTypes.Add("*." + typeName);
	}
}

// now build a list of all signed declarators

// first get add all declarators of type that is a typedefs of signed
CxList sgnDecls = Find_All_Declarators().FindByTypes((string[]) sgnTypes.ToArray(typeof(string)));
// then add all declarators of a signed type
sgnDecls.Add(Find_All_Declarators().FindByExtendedType("signed"));
sgnDecls -= typedefDecls;
// uses are decl instances
CxList sgnDeclUses = All.FindAllReferences(sgnDecls).FindByType(typeof(UnknownReference));
// remove all casted
CxList castIntoTypes = All.FindByFathers(All.FindByType(typeof(CastExpr))).FindByType(typeof(TypeRef));
sgnDeclUses -= sgnDeclUses.GetByAncs(castIntoTypes);
foreach(CxList cur in castIntoTypes){
	TypeRef g = (TypeRef) cur.data.GetByIndex(0);
	if (g == null || g.FullName == null) {
		continue;
	}
	string curName = g.FullName;
	// if irrelevant type, remove it from casted list
	if (!sgnTypes.Contains(curName) && !curName.StartsWith("signed") && !curName.Contains(" signed")){
		castIntoTypes -= cur;
	}	
}
// add back relevant castsed
sgnDeclUses.Add(All.GetByAncs(castIntoTypes.GetFathers()).FindByType(typeof(UnknownReference)));

CxList unaryExprs = All.FindByType(typeof(UnaryExpr));
CxList binaryExprs = All.FindByType(typeof(BinaryExpr));
CxList assignExprs = All.FindByType(typeof(AssignExpr));

// find expressions of type: {"<<", ">>", "&", "^", "|"}
CxList bitwiseOperators = binaryExprs.GetByBinaryOperator(BinaryOperator.BitwiseOr) +
	binaryExprs.GetByBinaryOperator(BinaryOperator.BitwiseAnd) +
	binaryExprs.GetByBinaryOperator(BinaryOperator.BitwiseXor) +
	binaryExprs.GetByBinaryOperator(BinaryOperator.ShiftLeft) +
	binaryExprs.GetByBinaryOperator(BinaryOperator.ShiftRight);
// find expressions of type: {"~"}
foreach (CxList cur in unaryExprs){
	UnaryExpr g = (UnaryExpr) cur.data.GetByIndex(0);
	if(g == null || g.Operator == null) {
		continue;
	}
	UnaryOperator curOp = g.Operator;
	if (curOp == UnaryOperator.OnesComplement){
		bitwiseOperators.Add(cur);
	}
}
// find expressions of type: {"<<=", ">>=", "&=", "^=", "|="}
foreach (CxList cur in assignExprs){
	AssignExpr g = (AssignExpr) cur.data.GetByIndex(0);
	if(g == null || g.Operator == null) {
		continue;
	}
	AssignOperator curOp = g.Operator;
	if ((curOp == AssignOperator.LeftShiftAssign) ||
		(curOp == AssignOperator.RightShiftAssign) ||
		(curOp == AssignOperator.BitwiseAndAssign) ||
		(curOp == AssignOperator.BitwiseOrAssign) ||
	(curOp == AssignOperator.ExclusiveOrAssign)){
		bitwiseOperators.Add(cur);
	}
}

result = sgnDeclUses.GetByAncs(bitwiseOperators);