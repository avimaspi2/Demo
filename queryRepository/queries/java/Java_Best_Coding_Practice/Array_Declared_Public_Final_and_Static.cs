// Find all arrays' definitions
CxList arrays = Find_IndexerRefs();
arrays = All.FindDefinition(arrays);
// Get all constant (i.e. "final") objects
CxList constants = Find_Constants();

// Add to "final" also "public" and "static"
CxList publicFinalStatic = constants.
	FindByFieldAttributes(Modifiers.Public).
	FindByFieldAttributes(Modifiers.Static);

// Get the array declarators that are in the public-final-static area
result = arrays.GetByAncs(publicFinalStatic);