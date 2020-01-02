CxList methods = Find_Methods();

string[] memberAccesses = new string[] {
	"File.lastModified",
	
	"Files.getLastModifiedTime",
	
	"BasicFileAttributes.creationTime",
	"BasicFileAttributes.lastAccessTime",
	"BasicFileAttributes.lastModifiedTime"
	};

foreach (string s in memberAccesses)
	result.Add(methods.FindByMemberAccess(s));