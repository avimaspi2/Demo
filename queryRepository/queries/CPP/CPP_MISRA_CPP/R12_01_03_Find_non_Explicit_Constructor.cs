/*
 MISRA CPP RULE 12-1-3
 ------------------------------
 This query searches for constructors with one parameter which is a 
 fundamental type, and returns those without the "explicit" keyword.

 The Example below shows code with vulnerability: 

    class Foo
	{
	public:
		Foo(int32_t a) { }; 			//Non-compliant
	};   
	
	class Bar
	{
	public:
	    explicit Bar (int32_t a) { }; 	//Compliant
	};

*/

//Build List with all of the fundamental types and their typedefs.
System.Collections.Generic.List<string> types = new System.Collections.Generic.List<string>();
types.Add("char");
types.Add("short");
types.Add("int");
types.Add("long");
types.Add("float");
types.Add("double");
types.Add("void");
types.Add("bool");

//Get all typedefs of fundamental types.
// start with all type objects
CxList basicTypes = All.FindByType(typeof(TypeRef));

// we only care about basic types
basicTypes = basicTypes.FindByName("char") +
	basicTypes.FindByName("short") +
	basicTypes.FindByName("int") +
	basicTypes.FindByName("long") +
	basicTypes.FindByName("float") +
	basicTypes.FindByName("double");

// remove redundent instances
basicTypes -= basicTypes.FindByFathers(All.FindByType(typeof(ObjectCreateExpr)));

// Find typedef'd types
CxList typedefDecl = All.FindByType(typeof(StringLiteral)).FindByName("CX_TYPEDEF");
typedefDecl = typedefDecl.GetAncOfType(typeof(VariableDeclStmt)) +
	typedefDecl.GetAncOfType(typeof(FieldDecl));
basicTypes = basicTypes.GetByAncs(typedefDecl);
typedefDecl = basicTypes.GetAncOfType(typeof(VariableDeclStmt)) +
	basicTypes.GetAncOfType(typeof(FieldDecl));
typedefDecl = Find_All_Declarators().GetByAncs(typedefDecl);

foreach(CxList typedef in typedefDecl) {
	types.Add(((Declarator) typedef.data.GetByIndex(0)).ShortName);
}

CxList cons = All.FindByType(typeof(ConstructorDecl));
cons -= cons.FindByFieldAttributes(Modifiers.Explicit);
CxList paramDeclColl = All.FindByType(typeof(ParamDeclCollection));
CxList paramCollParts = All.FindByType(typeof(ParamDecl)) + All.FindByType(typeof(TypeRef));

foreach (CxList con in cons) {
	CxList parCol = paramCollParts.GetByAncs(paramDeclColl.FindByFathers(con));
	CxList par = parCol.FindByType(typeof(ParamDecl));
	CxList typePar = parCol.FindByType(typeof(TypeRef));
	
	if (par.Count != 1) {//Check amount of parameters.
		continue;
	}
	//Check if parameter is fundamental.
	string typeStr = ((TypeRef) typePar.data.GetByIndex(0)).TypeName;
	if ( types.Contains(typeStr) ) {
		result.Add(con);
	}
}