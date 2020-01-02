/*
MISRA CPP RULE 3-9-2
------------------------------
This query searches for usage of basic types not wrapped by a typedef that indicates size and signedness
except for the definition of main()

	The Example below shows code with vulnerability: 

int i;

*/

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

// remove typedef'd types
CxList typedefAnc = All.FindByType(typeof(StringLiteral)).FindByName("CX_TYPEDEF").GetAncOfType(typeof(VariableDeclStmt))
	+ All.FindByType(typeof(StringLiteral)).FindByName("CX_TYPEDEF").GetAncOfType(typeof(FieldDecl));
basicTypes -= basicTypes.GetByAncs(typedefAnc);


// main() is exempt from this, return value type won't be returned since it receives a special name in DOM
// paramaters need to be dealt with
CxList mains = All.FindByType(typeof(MethodDecl)).FindByShortName("main");
CxList mainParamTypes = basicTypes.FindByFathers(All.FindByFathers(
	All.FindByFathers(mains).FindByType(typeof(ParamDeclCollection))));
CxList mainReturnValue = basicTypes.FindByFathers(mains);
basicTypes -= (mainParamTypes + mainReturnValue);

// Bit fields are also exempt
basicTypes -= basicTypes.FindByRegex(@"[\}|\w]+?\s*?:\s*?\d+?\s*?;", false, false, false);


result = basicTypes;