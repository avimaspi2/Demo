CxList ofHttpSessType = All.FindByType("HttpSession") - Find_TypeRef();
//Added a search for a getSession().invalidate() instead of just an HttpSession type
CxList session = Find_Methods().FindByMemberAccess(".getSession");
CxList sessionInvalid = session.GetMembersOfTarget().FindByMemberAccess(".invalidate");

result = ofHttpSessType.GetMembersOfTarget().FindByShortName("invalidate");
result.Add(sessionInvalid);