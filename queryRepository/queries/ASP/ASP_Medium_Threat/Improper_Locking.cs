CxList mutexWait = All.FindByMemberAccess("mutex.waitone");
CxList mutexRelease = All.FindByMemberAccess("mutex.releasemutex");
CxList mutex = mutexWait.GetTargetOfMembers();

result = mutex - mutex.DataInfluencingOn(mutexRelease);