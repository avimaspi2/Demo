CxList inputs = Find_Inputs();
CxList methods = Find_Methods();
CxList unkrefs = Find_UnknownReference();
List<string> osMethods = new List<string>{
		"os.access","os.chdir","os.chflags","os.chmod","os.chown","os.chroot",
		"os.fchdir","os.lchflags","os.lchmod",
		"os.lchown","os.link","os.listdir","os.lstat",
		"os.makedirs","os.mkdir","os.mkdirs","os.mkfifo","os.pathconf","os.readlink",
		"os.remove","os.removedirs","os.rename","os.renames","os.rmdir",
		"os.startfile","os.stat","os.statvfs","os.symlink","os.unlink","os.utime"};

CxList insecureMethods = All.NewCxList();
foreach (string cur in osMethods){
	insecureMethods.Add(methods.FindByMemberAccess(cur));
}

CxList sanitizers = Find_Sanitize();

List<string> sanitizeMethods = new List<string>{
		"abspath","startswith","lexists",
		"exists","isabs","isfile","isdir","realpath"};

sanitizers.Add(All.GetSanitizerByMethodInCondition(methods.FindByShortNames(sanitizeMethods, true)));

sanitizers.Add(methods.FindByParameters(sanitizers.GetParameters(methods)));

CxList insecureMethodsParameters = unkrefs.GetParameters(insecureMethods);

result = insecureMethodsParameters.InfluencedByAndNotSanitized(inputs, sanitizers);