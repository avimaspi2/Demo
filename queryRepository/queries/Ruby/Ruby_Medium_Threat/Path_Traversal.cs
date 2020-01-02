CxList inputs = Find_Interactive_Inputs();
CxList methods = Find_Methods();

CxList files = 
	methods.FindByMemberAccess("Dir.glob", false) +
	methods.FindByMemberAccess("File.delete", false) +
	methods.FindByMemberAccess("File.link", false) +
	methods.FindByMemberAccess("File.move", false) +
	methods.FindByMemberAccess("File.open", false) +
	methods.FindByMemberAccess("File.unlink", false) +
	methods.FindByMemberAccess("File.join", false) +
	methods.FindByMemberAccess("Pathname.delete", false) +
	methods.FindByMemberAccess("Pathname.new", false) +
	methods.FindByMemberAccess("Pathname.open", false) +
	methods.FindByMemberAccess("PStore.delete", false) +
	methods.FindByMemberAccess("PStore.fetch", false) +
	methods.FindByMemberAccess("PStore.new", false) +
	methods.FindByMemberAccess("PStore.root*", false) +
	methods.FindByMemberAccess("YAML.load", false) +
	methods.FindByShortName("chdir", false) +
	methods.FindByShortName("chmod", false) +
	methods.FindByShortName("chown", false) +
	methods.FindByShortName("chroot", false) +
	methods.FindByShortName("lchmod", false) +
	methods.FindByShortName("lchown", false) +
	methods.FindByShortName("rename", false) +
	methods.FindByShortName("rmdir", false) +
	methods.FindByShortName("safe_unlink", false) +
	methods.FindByShortName("symlink", false) +
	methods.FindByShortName("syscopy", false) +
	methods.FindByShortName("sysopen", false) +
	methods.FindByShortName("truncate", false);	
files = files.GetTargetOfMembers().GetMembersOfTarget();
CxList numberSanitizer = 
	methods.FindByShortName("round", false) + 
	methods.FindByShortName("doubleval", false) +
	methods.FindByShortName("strlen", false);

result = files.InfluencedByAndNotSanitized(inputs, Find_Integers() + numberSanitizer);