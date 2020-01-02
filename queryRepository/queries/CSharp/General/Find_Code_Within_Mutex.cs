CxList mutex = All.FindByType("Mutex");
CxList semaphore = All.FindByType("Semaphore");
CxList waitOne = All.FindByShortName("WaitOne");
CxList memberOfMutex = (mutex+semaphore).GetMembersOfTarget();
CxList waitOneOfMutex = memberOfMutex* waitOne;

result=All.GetByAncs(waitOneOfMutex.GetAncOfType(typeof(StatementCollection)));