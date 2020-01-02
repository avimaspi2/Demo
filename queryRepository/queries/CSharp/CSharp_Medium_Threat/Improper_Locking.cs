CxList typeMutex = All.FindByType("Mutex");

CxList mutexList = typeMutex.GetMembersOfTarget();

CxList mutexClose = mutexList.FindByName("*.Close");
mutexClose.Add(mutexList.FindByName("*.Dispose"));
mutexClose.Add(mutexList.FindByName("*.ReleaseMutex"));

CxList mutexCons = typeMutex.FindByType(typeof(ObjectCreateExpr));

CxList mutexOpen = mutexList.FindByMemberAccess("Mutex.WaitOne");

mutexOpen = typeMutex.FindDefinition(mutexOpen.GetTargetOfMembers());
mutexOpen.Add(mutexCons);
mutexOpen -= mutexOpen.FindByType(typeof(Declarator));

result = mutexOpen - mutexOpen.DataInfluencingOn(mutexClose);