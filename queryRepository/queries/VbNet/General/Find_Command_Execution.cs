result = All.FindByName("*.Process.Start", false);
result.Add(All.FindByName("Process.Start", false));
result.Add(All.FindByMemberAccess("Process.Start", false));