/// <summary>
/// Find the PhoneGap File Read API
/// </summary>
CxList methods = Find_Methods();
CxList ObjCreates = Find_ObjectCreations();

// relevant FileReader methods
CxList reads = methods.FindByShortNames(new List<string>{
		"readAsArrayBuffer", "readAsBinaryString", "readAsDataURL", "readAsText"});
result.Add(reads.GetTargetOfMembers().FindByType("FileReader").GetMembersOfTarget());

// Relevant *Entry Methods
CxList entryReads = methods.FindByShortNames(new List<string>{
		"getMetadata", "toURL", "toURI", "getParent", 
		"createReader", "getDirectory", "getFile"});
result.Add(entryReads.GetTargetOfMembers()
	.FindByTypes(new string[]{"Entry", "FileEntry", "DirectoryEntry"}).GetMembersOfTarget());

// Objects and methods that acquire data from filesystem
result.Add(methods.FindByShortName("requestFileSystem", true));
result.Add(ObjCreates.FindByShortName("FileSystem", true));