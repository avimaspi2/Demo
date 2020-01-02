CxList methods = Find_Methods();
CxList getInstance = methods.FindByShortName("getInstance");
CxList exec = 
	methods.FindByMemberAccess("Runtime.exec") + 
	methods.FindByMemberAccess("getRuntime.exec") +
	methods.FindByMemberAccess("System.exec") +
	methods.FindByMemberAccess("Executor.safeExec");
CxList conditions = Find_Conditions();

CxList after = All.FindByMemberAccess("Calendar.after");
after = after.DataInfluencedBy(getInstance);

CxList afterInCondition = conditions.DataInfluencedBy(after) + conditions * after;

result = exec.GetByAncs(afterInCondition.GetFathers());