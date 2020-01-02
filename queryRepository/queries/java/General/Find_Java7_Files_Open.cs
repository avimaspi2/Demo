/*
	Most of the methods from java.nio.file.Files handle files. The ones that doesn't, are sanitized by the
	pathTraversalSanitizers.
*/
CxList methods = Find_Methods();
CxList filesMethods = methods.FindByMemberAccess("Files.*");
result = filesMethods.FindByShortNames(new List<string>{
		"copy", "createLink", "createSymbolicLink", "delete*", "setOwner"});