CxList baseRefs = Find_BaseRef();

CxList superRun = All.FindByFathers(baseRefs.GetFathers()).FindByShortName("run");
result = All.FindByMemberAccess("Thread.run") - superRun;