// The purpose of the query is to found 
// All database access input in Entity Framework (EF)

CxList typeOfObjectQuery = All.FindByType("ObjectQuery", false);
CxList propertyDeclaration = All.FindByType(typeof(PropertyDecl)) * typeOfObjectQuery.GetFathers();

// Find all DB member access
result = All.FindAllReferences(propertyDeclaration) - propertyDeclaration - All.FindAllReferences(propertyDeclaration).FindByType(typeof(TypeRef));