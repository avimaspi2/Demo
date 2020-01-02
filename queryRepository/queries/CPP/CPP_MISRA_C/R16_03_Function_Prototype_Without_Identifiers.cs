/*
MISRA C RULE 16-3
------------------------------
This query searches for prototypes (such as function declarations) without - identifiers for all paramaters

	The Example below shows code with vulnerability: 

short sum (int, int);

*/

CxList methodDefs = All.FindByType(typeof(StatementCollection)).GetFathers().FindByType(typeof(MethodDecl));
CxList methodPrototypes = All.FindByType(typeof(MethodDecl)) - methodDefs;
CxList typeRef = All.FindByType(typeof(TypeRef));
CxList emptyTypeParamaters = typeRef.FindByShortName("").GetAncOfType(typeof(ParamDecl));
CxList voidTypeParamaters = typeRef.FindByShortName("void").GetAncOfType(typeof(ParamDecl));

// we only concerned with prototype (not definition) paramaters
CxList emptyIdentParamaters = All.FindByType(typeof(ParamDecl)).FindByShortName("").
	GetByAncs(methodPrototypes);

// Almost all paramaters with empty name are empty identifiers and should be addes
result.Add(emptyIdentParamaters);

// except "..." type paramaters, which are supposed to have no name
result -= emptyTypeParamaters;

// and "void" paramaters, which aren't supposed to have a name
result -= voidTypeParamaters;