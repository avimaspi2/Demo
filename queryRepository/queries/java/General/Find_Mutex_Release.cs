CxList mutexRelease = All.FindByMemberAccess("Lock.unlock");
mutexRelease.Add(mutexRelease.GetTargetOfMembers().GetTargetOfMembers());
result = mutexRelease;