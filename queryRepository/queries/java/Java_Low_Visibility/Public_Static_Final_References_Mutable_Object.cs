// Get all constant (i.e. "final") objects
CxList constants = Find_Constants();

// Add to "final" also "public" and "static"
CxList publicFinalStatic = constants.
	FindByFieldAttributes(Modifiers.Public).
	FindByFieldAttributes(Modifiers.Static);

//Added part
//Find all objects that are not primitive type
CxList rankSpecifier = All.FindByType(typeof (RankSpecifier));
rankSpecifier.Add(Find_Collections());

CxList stringBuild = publicFinalStatic.FindByType("StringBuilder");

//leave only non primitive objects inside "publicFinalStatic"
//rankSpecifier.GetAncOfType(typeof(ConstantDecl)) returns nothing
publicFinalStatic = publicFinalStatic * ((rankSpecifier.GetAncOfType(typeof(ConstantDecl)) + rankSpecifier.GetAncOfType(typeof(FieldDecl))) * Find_Constants());
publicFinalStatic.Add(stringBuild);
//End of added part

// Find all the references of the public final static
CxList pfsRef = All.FindAllReferences(publicFinalStatic);
// Leave only the ones under a return statement
pfsRef = pfsRef.GetByAncs(All.FindByType(typeof(ReturnStmt)));
// Remove members (such as date.getTime(), which are OK with us
pfsRef -= pfsRef.GetMembersOfTarget().GetTargetOfMembers();

// Now make sure we leave only the ones that are directly under the return value (no manipulation)
CxList pfsInReturn = pfsRef.GetByAncs(pfsRef.GetFathers().FindByType(typeof(ReturnStmt)));

// ...and return the result
result = pfsInReturn;