// Find all private methods
CxList privateMethods = Find_MethodDecls().FindByFieldAttributes(Modifiers.Private);
// Get all their references
CxList privateReferences = All.FindAllReferences(privateMethods) - privateMethods;
// And leave only the ones that are never called
result = privateMethods - privateMethods.FindDefinition(privateReferences);