CxList getSession = All.FindByMemberAccess("request.getSession");
getSession.Add(All.FindByName("*request.getSession"));
getSession.Add(All.FindByName("*Request.getSession"));

CxList getAttr = All.FindByMemberAccess("HttpServletResponse.getAttribute");
getAttr.Add(All.FindByMemberAccess("HttpServletRequest.getAttribute"));
getAttr.Add(All.FindByName("*request.getAttribute"));
getAttr.Add(All.FindByName("*Request.getAttribute"));

getAttr.Add(getSession.GetMembersOfTarget().FindByShortName("getAttribute"));
getAttr.Add(All.FindByMemberAccess("Session.getAttribute"));
getAttr.Add(All.FindByMemberAccess("HttpSession.getAttribute"));
getAttr.Add(All.FindByName("*session.getAttribute", false));
getAttr.Add(All.FindByName("*session.get", false));
getAttr.Add(All.FindByMemberAccess("session.get"));

result = getAttr;