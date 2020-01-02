CxList getSession = 
	All.FindByMemberAccess("request.getSession") +
	All.FindByName("*request.getSession") +
	All.FindByName("*Request.getSession");

CxList getAttr = 
	All.FindByMemberAccess("HttpServletResponse.getAttribute") +
	All.FindByMemberAccess("HttpServletRequest.getAttribute") +
	All.FindByName("*request.getAttribute") +
	All.FindByName("*Request.getAttribute");

getAttr.Add(getSession.GetMembersOfTarget().FindByShortName("getAttribute"));
getAttr.Add(All.FindByMemberAccess("Session.getAttribute"));
getAttr.Add(All.FindByName("*session.getAttribute", false));
getAttr.Add(All.FindByName("*session.get", false));
getAttr.Add(All.FindByMemberAccess("session.get"));

result = getAttr;