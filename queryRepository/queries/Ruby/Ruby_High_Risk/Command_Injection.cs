/*
still missing:
		`#{command}`
		@whois = %x[whois #{dom}]
*/


CxList methods = Find_Methods();

CxList commands = methods.FindByShortNames(new List<string> {"exec", "syscall", "system", "spawn"});
// Filter out members of classes which are called "command/exec" etc.
commands -= commands.GetTargetOfMembers().GetMembersOfTarget();

commands.Add(methods.FindByMemberAccess("IO.popen"));
commands.Add(methods.FindByMemberAccess("Open3.popen3"));
commands.Add(methods.FindByMemberAccess("Open3.popen2"));
commands.Add(methods.FindByMemberAccess("Open3.popen2e"));
commands.Add(methods.FindByMemberAccess("Open3.pipeline"));
commands.Add(methods.FindByMemberAccess("Open3.capture2"));
commands.Add(methods.FindByMemberAccess("Open3.capture3"));
commands.Add(methods.FindByMemberAccess("Open3.pipeline_start"));
commands.Add(methods.FindByMemberAccess("Open3.pipeline_w"));
commands.Add(methods.FindByMemberAccess("Open3.pipeline_r"));
commands.Add(methods.FindByMemberAccess("Open3.pipeline_rw"));
commands.Add(methods.FindByMemberAccess("Open4.popen4"));
commands.Add(methods.FindByMemberAccess("Open4.spawn"));
commands.Add(methods.FindByMemberAccess("PTY.getpty"));
commands.Add(methods.FindByMemberAccess("PTY.spawn"));
commands.Add(methods.FindByMemberAccess("Kernel.exec"));
commands.Add(methods.FindByMemberAccess("*Process.spawn"));
commands.Add(methods.FindByMemberAccess("Process.exec"));
commands.Add(methods.FindByMemberAccess("Subexec.run")); 
commands.Add(methods.FindByMemberAccess("subexec.run"));

CxList inputs = Find_Interactive_Inputs();
CxList sanitize = (All.GetByAncs(All.GetParameters(commands)) - All.GetByAncs(All.GetParameters(commands, 0))) +
	Find_Integers();


result = commands.InfluencedByAndNotSanitized(inputs, sanitize);