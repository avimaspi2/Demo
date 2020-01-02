/*
MISRA CPP RULE 5-0-10
------------------------------
This query searches for bitwise operators: ~, <<, <<=
used on an operand with underlying signed type 

	The Example below shows code with vulnerability: 

   uint8_t num;
   uint8_t temp_8; 
   uint16_t temp_16; 
   uint16_t mod; 
   temp_8 = ( ~num ) >> 4;   // Non-compliant
   temp_16 = ( ( num << 4 ) & mod ) >> 6; // Non-compliant    

*/

// first we build a list of all unsigned typedefs
CxList typedefDecls = All.FindByType(typeof(StringLiteral)).FindByName("CX_TYPEDEF").GetFathers();
typedefDecls = typedefDecls.FindByType("char") + typedefDecls.FindByType("short");
CxList typedefSgnDecls = typedefDecls.FindByExtendedType("unsigned");
ArrayList sgnTypes = new ArrayList();
sgnTypes.Add("char");
sgnTypes.Add("*.char");
sgnTypes.Add("short");
sgnTypes.Add("*.short");

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

// now build a list of all unsigned declarators

// first get add all declarators of type that is a typedefs of unsigned
CxList sgnDecls = Find_All_Declarators().FindByTypes((string[]) sgnTypes.ToArray(typeof(string))) - typedefDecls;
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
	if (!sgnTypes.Contains(curName) && !curName.StartsWith("unsigned") && !curName.Contains("unsigned"))
	{
		castIntoTypes -= cur;
	}	
}

// add back relevant castsed
sgnDeclUses.Add(All.GetByAncs(castIntoTypes.GetFathers()).FindByType(typeof(UnknownReference)));

CxList unaryExprs = All.FindByType(typeof(UnaryExpr));
CxList binaryExprs = All.FindByType(typeof(BinaryExpr));
CxList assignExprs = All.FindByType(typeof(AssignExpr));

// find expressions of type: "<<"
CxList bitwiseOperators = binaryExprs.GetByBinaryOperator(BinaryOperator.ShiftLeft);

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

//find expressions of type: "<<="
foreach (CxList cur in assignExprs){
	AssignExpr g = (AssignExpr) cur.data.GetByIndex(0);
	if(g == null || g.Operator == null) {
		continue;
	}
	AssignOperator curOp = g.Operator;
	if (curOp == AssignOperator.LeftShiftAssign)
	{
		bitwiseOperators.Add(cur);
	}
}

//return only the non casted operations
CxList staticCasts = All.FindByType(typeof(MethodInvokeExpr)).FindByShortName("*static_cast*");
CxList nonCasted = sgnDeclUses.GetByAncs(bitwiseOperators);
CxList casted = All.GetByAncs(staticCasts);
nonCasted = nonCasted - casted;

result = nonCasted;