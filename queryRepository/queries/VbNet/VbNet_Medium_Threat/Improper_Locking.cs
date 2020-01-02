CxList mutexWait = All.FindByMemberAccess("Mutex.WaitOne", false);
CxList mutexRelease = All.FindByMemberAccess("Mutex.ReleaseMutex", false);
CxList mutex = mutexWait.GetTargetOfMembers();

result = mutex - mutex.DataInfluencingOn(mutexRelease);