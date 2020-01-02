CxList Inputs = Find_Interactive_Inputs();
CxList sleep = All.FindByName("*Thread.sleep");

CxList tooltipDelay = Find_Jsp_Tags().GetMembersOfTarget().FindByMemberAccess("tooltipDelay.*");//.GetParameters(Find_Jsp_Tags());
tooltipDelay = Find_Methods().GetParameters(tooltipDelay);

result = (sleep + tooltipDelay).DataInfluencedBy(Inputs);