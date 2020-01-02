//checks if there are locks that are not unlocked.

CxList mutexWait = All.FindByMemberAccess("Lock.lock") + All.FindByMemberAccess("Lock.tryLock");
CxList mutexRelease = All.FindByMemberAccess("Lock.unlock");

CxList mutLock = All.NewCxList();
foreach(CxList mut in mutexWait)
{
	CxList mutex = mut.GetTargetOfMembers().GetTargetOfMembers();
	if(mutex.Count == 0)
	{
                                
		mutLock.Add(mut.GetTargetOfMembers());
	}
	else
	{
		mutLock.Add(mut.GetTargetOfMembers().GetTargetOfMembers());
                                
	}
}

result = mutLock - 
	mutLock.DataInfluencingOn(mutexRelease) - 
	All.FindAllReferences(mutexRelease.GetTargetOfMembers().GetTargetOfMembers());