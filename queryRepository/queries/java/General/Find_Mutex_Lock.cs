CxList mutexWait = All.FindByMemberAccess("Lock.lock");
mutexWait.Add(All.FindByMemberAccess("Lock.tryLock"));

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
result = mutLock;