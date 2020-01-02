/*
MISRA C RULE 12-9
------------------------------
This query searches for instances of the unary '-' applied to unsigned objects

	The Example below shows code with vulnerability: 

int8_t   mc2_1209_8a;
uint8_t  mc2_1209_u8a;
uint8_t  mc2_1209_u8b;

mc2_1209_8a = -mc2_1209_u8a;                     
mc2_1209_8a = -( mc2_1209_u8a + mc2_1209_u8b )

*/

// first we build a list of all unsigned typedefs
CxList typedefDecls = All.FindByType(typeof(StringLiteral)).FindByName("CX_TYPEDEF").GetFathers();
CxList typedefUnsgnDecls = typedefDecls.FindByExtendedType("unsigned");
ArrayList unsgnTypes = new ArrayList();
foreach(CxList cur in typedefUnsgnDecls){
	Declarator g = (Declarator) cur.data.GetByIndex(0);
	if(g == null || g.Name == null) {
		continue;
	}
	string typeName = g.Name;
	if (!unsgnTypes.Contains(typeName)){
		unsgnTypes.Add(typeName);
		unsgnTypes.Add("*." + typeName);
	}
}

// now build a list of all unsigned declarators

// first get add all declarators of type that is a typedefs of signed
CxList unsgnDecls = Find_All_Declarators().FindByTypes((string[]) unsgnTypes.ToArray(typeof(string)));

// then add all declarators of a signed type
unsgnDecls.Add(Find_All_Declarators().FindByExtendedType("unsigned"));
unsgnDecls -= typedefDecls;
CxList unsgnDeclUses = All.FindAllReferences(unsgnDecls).FindByType(typeof(UnknownReference));
// remove all casted
unsgnDeclUses -= unsgnDeclUses.GetByAncs(All.FindByType(typeof(CastExpr)));
// add back correct casted only
CxList castIntoTypes = All.FindByFathers(All.FindByType(typeof(CastExpr))).FindByType(typeof(TypeRef));
foreach(CxList cur in castIntoTypes){
	TypeRef g = (TypeRef) cur.data.GetByIndex(0);
	if (g == null || g.FullName == null) {
		continue;
	}
	string curName = g.FullName;
	// if irrelevant type, remove it from list
	if (!unsgnTypes.Contains(curName) && !curName.Contains("unsigned")){
		castIntoTypes -= cur;
	}
}
// add correct casts to uses list
unsgnDeclUses.Add(All.GetByAncs(castIntoTypes.GetFathers()).FindByType(typeof(UnknownReference)));

// find expressions of unary type '-'
CxList unaryMinusOps = All.NewCxList();
CxList unaryExprs = All.FindByType(typeof(UnaryExpr));
foreach (CxList cur in unaryExprs){
	UnaryExpr g = (UnaryExpr) cur.data.GetByIndex(0);
	if (g == null || g.Operator == null) {
		continue;
	}
	UnaryOperator curOp = g.Operator;
	if (curOp == UnaryOperator.UnaryNegation){
		unaryMinusOps.Add(cur);
	}
}

result = unsgnDeclUses.GetByAncs(unaryMinusOps);