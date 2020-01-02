// Find Sensitive data field (non boolean)
CxList sensitiveData = Find_Personal_Info();
sensitiveData = sensitiveData * Find_Field_Decl();
sensitiveData -= sensitiveData.FindByType("boolean");
sensitiveData -= sensitiveData.FindByType("bool");
// Find their classes
CxList sensitiveClasses = sensitiveData.GetAncOfType(typeof(ClassDecl));

CxList methodDecl = Find_MethodDeclaration();
// Find "writeObject" that is final and has exactly one parameter
CxList writeObject = methodDecl.FindByShortName("writeObject").FindByFieldAttributes(Modifiers.Sealed);
CxList writeObjectParams0 = All.GetParameters(writeObject, 0);
CxList writeObjectParams1 = All.GetParameters(writeObject, 1);
writeObject =  
	writeObjectParams0.GetAncOfType(typeof(MethodDecl)) - 
	writeObjectParams1.GetAncOfType(typeof(MethodDecl));

// Leave only writeObject that throws an exception
CxList throwStmt = All.FindByType(typeof(ThrowStmt));
writeObject = writeObject * writeObject.GetMethod(throwStmt);

// The problematic classes are the ones that are not implementing the above method
CxList nonSerializableClasses = writeObject.GetAncOfType(typeof(ClassDecl));
CxList problematicClasses = sensitiveClasses - nonSerializableClasses;
// Remove explicitly serialized classes (this is a "potential" query)
problematicClasses -= problematicClasses.InheritsFrom("Serializable");

CxList problematicClassesSensitiveData = sensitiveData.GetByAncs(problematicClasses);

// For each problematic class connect the class with the sensitive data inside it
foreach (CxList problematicClass in problematicClasses)
{
	CxList sensitive = problematicClassesSensitiveData.GetByAncs(problematicClass);
	result.Add(sensitive.ConcatenateAllSources(problematicClass));
}