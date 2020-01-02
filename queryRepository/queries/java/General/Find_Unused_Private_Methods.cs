// Find all private methods
CxList privateMethods = Find_MethodDeclaration().FindByFieldAttributes(Modifiers.Private);
// Remove java.io.serializable interface
privateMethods = privateMethods - privateMethods.FindByShortName("writeObject");
privateMethods = privateMethods - privateMethods.FindByShortName("readObject");
// Remove toString
privateMethods = privateMethods - privateMethods.FindByShortName("toString");
// Get all their references
CxList privateReferences = All.FindAllReferences(privateMethods) - privateMethods;
// And leave only the ones that are never called
result = privateMethods - privateMethods.FindDefinition(privateReferences);