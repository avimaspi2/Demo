CxList methods = Find_Methods();

string[] memberAccesses = new string[] {
	"File.setExecutable",
	"File.setLastModified",
	"File.setReadable",
	"File.setReadOnly",
	"File.setWritable",

	"Files.setAttribute",
	"Files.setLastModifiedTime",	
	"Files.setOwner",
	"Files.setPosixFilePermissions"
	};

foreach (string s in memberAccesses)
	result.Add(methods.FindByMemberAccess(s));