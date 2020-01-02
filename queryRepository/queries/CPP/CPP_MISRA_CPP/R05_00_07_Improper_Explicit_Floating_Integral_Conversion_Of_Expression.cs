/*
MISRA CPP RULE 5-0-7
------------------------------
This query searches for usage of explicit floating-integral conversions of a cvalue expression.
Casting the result of an expression does not change it's evaluated type, This may contradict developer expectations.

	The Example below shows code with vulnerability: 

int16_t   num1; 
int16_t   num2;

float32_t num3 = static_cast< float32_t > (num1/num2);

*/


// first we build a list of all integer and floating point typedefs
CxList typedefDecls = All.FindByType(typeof(StringLiteral)).FindByName("CX_TYPEDEF").GetFathers();

CxList typedefSgnDecls = 
	typedefDecls.FindByType("char") + 
	typedefDecls.FindByType("double") + 
	typedefDecls.FindByType("float") + 
	typedefDecls.FindByType("int") +
	typedefDecls.FindByType("long") + 
	typedefDecls.FindByType("short");


ArrayList sgnTypes = new ArrayList();
sgnTypes.Add("char");
sgnTypes.Add("*.char");
sgnTypes.Add("double");
sgnTypes.Add("*.double");
sgnTypes.Add("float");
sgnTypes.Add("*.float");
sgnTypes.Add("short");
sgnTypes.Add("*.short");
sgnTypes.Add("int");
sgnTypes.Add("*.int");
sgnTypes.Add("long");
sgnTypes.Add("*.long");

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

// now build a list of all declarators

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
	if (!sgnTypes.Contains(curName))
	{
		castIntoTypes -= cur;
	}	
}

// add back relevant castsed
sgnDeclUses.Add(All.GetByAncs(castIntoTypes.GetFathers()).FindByType(typeof(UnknownReference)));


CxList binaryExprs = All.FindByType(typeof(BinaryExpr));

// find expressions of devision type: "/"
CxList divideOperators = binaryExprs.GetByBinaryOperator(BinaryOperator.Divide);

//return only the non casted operations
CxList staticCasts = All.FindByType(typeof(MethodInvokeExpr)).FindByShortName("*static_cast*");
CxList castedParams = All.GetParameters(staticCasts);
divideOperators = divideOperators.GetByAncs(castedParams);

result = sgnDeclUses.GetByAncs(divideOperators);