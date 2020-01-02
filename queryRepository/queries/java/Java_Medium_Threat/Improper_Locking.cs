//Checks if there are locks that are not unlocked.

CxList mutexRelease = Find_Mutex_Release();
CxList mutLock = Find_Mutex_Lock();
CxList vulnerable = mutLock - mutLock.DataInfluencingOn(mutexRelease) - 
	All.FindAllReferences(mutexRelease);

//In case the flow is not correct (in cases with try/finally blocks)
//	check if an unlock() is in the same method as lock() 
//	and that both have the same target
//If a matching unlock() is found for the lock() it will be removed from the vulnerable list
foreach (CxList mutexLock in vulnerable)
{
	CxList scope = mutexLock.GetAncOfType(typeof(MethodDecl));
	CxList unlockInScope = mutexRelease.GetByAncs(scope);
	CxList targetUnlock = unlockInScope.GetTargetOfMembers();
	if(targetUnlock.Count > 0)
	{
		CxList match = targetUnlock.FindAllReferences(mutexLock);
		if (match.Count > 0)
		{
			vulnerable -= mutexLock;	
		}
	}
}

result = vulnerable;