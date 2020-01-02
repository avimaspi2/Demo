CxList methods = Find_Methods();

string[] memberAccesses = new string[] {
	"File.canExecute",
	"File.canRead",
	"File.canWrite",
	"Files.getOwner",
	"Files.getPosixFilePermissions"
	};

foreach (string s in memberAccesses)
	result.Add(methods.FindByMemberAccess(s));