/*
The query finds all places where a Security Manager is being set programatically with a null argument.
*/

CxList nulls = All.FindByAbstractValue(_ => _ is NullAbstractValue);
CxList setSecurityManagers = Find_Methods().FindByMemberAccess("System.setSecurityManager");
result = nulls.InfluencingOn(setSecurityManagers).ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);