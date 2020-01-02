CxList methods = Find_Methods();

string[] memberAccesses = new string[] {
	"File.getFreeSpace",
	"File.getTotalSpace",
	"File.getUsableSpace",
	"File.length",
	
	"Files.size"
	};

foreach (string s in memberAccesses)
	result.Add(methods.FindByMemberAccess(s));