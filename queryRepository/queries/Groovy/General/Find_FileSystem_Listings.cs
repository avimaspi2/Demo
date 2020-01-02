CxList methods = Find_Methods();

string[] memberAccesses = new string[] {
	"File.list",
	"File.listFiles",
	"File.listRoots",
	
	"FileSystem.getRootDirectories"
	};

foreach (string s in memberAccesses)
	result.Add(methods.FindByMemberAccess(s));