CxList getSession = 
	All.FindByMemberAccess("request.getSession") +
	All.FindByName("*request.getSession") +
	All.FindByName("*Request.getSession");

CxList setAttr = 
	All.FindByMemberAccess("HttpServletResponse.setAttribute") +
	All.FindByMemberAccess("HttpServletRequest.setAttribute") +
	All.FindByName("*response.setAttribute") +
	All.FindByName("*Response.setAttribute");
setAttr.Add(getSession.GetMembersOfTarget().FindByShortName("setAttribute"));
setAttr.Add(All.FindByMemberAccess("Session.setAttribute"));
setAttr.Add(All.FindByName("*session.setAttribute", false));
setAttr.Add(All.FindByName("*session.put", false));
setAttr.Add(All.FindByMemberAccess("session.put"));
setAttr.Add(All.FindByMemberAccess("Model.addAttribute"));

result = setAttr;