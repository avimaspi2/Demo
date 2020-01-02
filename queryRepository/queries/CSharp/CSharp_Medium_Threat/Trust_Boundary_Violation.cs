CxList setAttr = All.FindByMemberAccess("Session.Add");

CxList httpSession = All.FindByMemberAccess("HttpContext.Current").GetMembersOfTarget();
CxList Sessions = httpSession.FindByShortName("Session*") - httpSession.FindByShortName("Session");

CxList assign = Sessions.GetFathers().GetFathers().FindByType(typeof (AssignExpr));

setAttr.Add(All.GetByAncs(assign).FindByAssignmentSide(CxList.AssignmentSide.Right));

CxList sanitize = Find_Sanitize() + 
	All.FindByShortName("Session");

CxList input = Find_Inputs();

result = setAttr.InfluencedByAndNotSanitized(input, sanitize);