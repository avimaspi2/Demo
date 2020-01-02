/*
MISRA C RULE 12-12
------------------------------
This query searches for instances of bitwise operators (~, <<, <<=, >>, >>=, &, &=, ^, ^=, |, |=) 
used on an operand with underlying signed type 

	The Example below shows code with vulnerability: 

float32_t mc2_1212_b;
uint32_t mc2_1212_c;
mc2_1212_c = * (uint32_t *) &mc2_1212_b;

union{
float32_t real;
uint32_t decimal;
} mc2_1212_a;

*/

// first we build a list of all floating point typedefs
CxList typedefDecls = All.FindByType(typeof(StringLiteral)).FindByName("CX_TYPEDEF").GetFathers();
CxList typedefFloatDecls = typedefDecls.FindByType("float") + typedefDecls.FindByType("double");

ArrayList floatTypes = new ArrayList();
floatTypes.Add("float");
floatTypes.Add("*.float");
floatTypes.Add("double");
floatTypes.Add("*.double");
foreach(CxList cur in typedefFloatDecls){
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

// first get add all declarators of type that is a typedefs of floating point
string[] floatTypeStrings = (string[]) floatTypes.ToArray(typeof(string));
CxList floatDecls = Find_All_Declarators().FindByTypes(floatTypeStrings);

// then add all declarators of a floating point
floatDecls.Add(Find_All_Declarators().FindByType("float") + typedefDecls.FindByType("double"));
floatDecls -= typedefDecls;

// create strings with all floating declaration informations
string allFloatDeclNames = "";
string allFloatDeclTypeAndNames = "";
foreach (CxList cur in floatDecls){
	CSharpGraph g = cur.data.GetByIndex(0) as CSharpGraph;
	if (g == null || g.ShortName == null || g.TypeName == null) {
		continue;
	}
	allFloatDeclNames+= "|" + g.ShortName;
	allFloatDeclTypeAndNames+= "|" + g.TypeName + @"\s+?" + g.ShortName;
}

// if we had any float decls, add all their names to the regex we search
if (allFloatDeclNames.Length > 0){
	allFloatDeclNames = "(" + allFloatDeclNames.Substring(1) + ")";
	allFloatDeclTypeAndNames = "(" + allFloatDeclTypeAndNames.Substring(1) + ")";
}

// add floating unions containing floating points
result.Add(All.FindByRegex(@"union[^{]*{[^}]*" + allFloatDeclTypeAndNames + "[^}]*}", false, false, false, All.NewCxList()));

// add floating point pointers casted to other pointer type
CxList casts = All.FindByType(typeof(CastExpr));
casts -= casts.FindByTypes(floatTypeStrings).GetFathers();
result.Add(casts.FindByRegex(@"\*\s*\)[\(\s*]\&" + allFloatDeclNames, false, false, false));