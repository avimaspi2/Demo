result = All.FindByName("*.Process.Start");
result.Add(All.FindByName("Process.Start"));
result.Add(All.FindByMemberAccess("Process.Start"));

CxList processsWriteLines = All.FindByMemberAccess("Process.StandardInput")
							   .GetMembersOfTarget()
							   .FindByShortName("Write*");
result.Add(processsWriteLines);