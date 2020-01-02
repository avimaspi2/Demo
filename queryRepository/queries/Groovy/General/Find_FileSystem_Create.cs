CxList methods = Find_Methods();

string[] memberAccesses = new string[] {
	"File.createNewFile",
	"File.createTempFile",
	"File.mkdir",
	"File.mkdirs",
	
	"Files.createDirectories",
	"Files.createDirectory",
	"Files.createFile",
	"Files.createLink",
	"Files.createSymbolicLink",
	"Files.createTempDirectory",
	"Files.createTempFile"
	};

foreach (string s in memberAccesses)
	result.Add(methods.FindByMemberAccess(s));