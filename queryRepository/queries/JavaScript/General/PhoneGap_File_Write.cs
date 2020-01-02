/// <summary>
/// Find methods from PhoneGap that allow to Write to filesystem
/// </summary>
CxList methods = Find_Methods();

// Relevant methods from FileWriter
CxList reads = methods.FindByShortNames(new List<string>{"write", "truncate",});
result.Add(reads.GetTargetOfMembers().FindByType("FileWriter").GetMembersOfTarget());

// Relevant methods from *Entry
CxList EntryWrites = methods.FindByShortNames(new List<string>{	
		"setMetadata", "moveTo", "copyTo",  "remove",
		"removeRecursively", "createWriter", "file" });
result.Add(EntryWrites.GetTargetOfMembers()
	.FindByTypes(new string[]{"Entry", "FileEntry", "DirectoryEntry"}).GetMembersOfTarget());