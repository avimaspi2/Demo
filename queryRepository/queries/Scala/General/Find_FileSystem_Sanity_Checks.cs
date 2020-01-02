CxList methods = Find_Methods();

string[] memberAccesses = new string[] {
	"File.exists",
	"File.isDirectory",
	"File.isFile",
	"File.isHidden",
	
	"Files.exists",
	"Files.isDirectory",
	"Files.isHidden",
	"Files.isRegularFile",
	"Files.isSymbolicLink",
	"Files.notExists",
	
	"FileSystem.isOpen"
	};

foreach (string s in memberAccesses)
	result.Add(methods.FindByMemberAccess(s));