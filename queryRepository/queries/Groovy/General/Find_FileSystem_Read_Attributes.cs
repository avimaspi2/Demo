CxList methods = Find_Methods();

string[] memberAccesses = new string[] {	
	"Files.getAttribute",
	"Files.probeContentType",
	"Files.readAttributes",
	"Files.readSymbolicLink"	
	};

foreach (string s in memberAccesses)
	result.Add(methods.FindByMemberAccess(s));