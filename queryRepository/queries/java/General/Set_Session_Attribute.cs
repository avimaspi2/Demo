CxList getSession = All.FindByMemberAccess("request.getSession");

getSession.Add(All.FindByName("*request.getSession"));
getSession.Add(All.FindByName("*Request.getSession"));

CxList setAttr = All.FindByMemberAccess("HttpServletResponse.setAttribute");

setAttr.Add(All.FindByMemberAccess("HttpServletRequest.setAttribute"));

setAttr.Add(All.FindByName("*response.setAttribute")); 
setAttr.Add(All.FindByName("*Response.setAttribute"));
setAttr.Add(getSession.GetMembersOfTarget().FindByShortName("setAttribute"));
setAttr.Add(All.FindByMemberAccess("Session.setAttribute"));
setAttr.Add(All.FindByMemberAccess("HttpSession.setAttribute"));
setAttr.Add(All.FindByName("*session.setAttribute", false));
setAttr.Add(All.FindByMemberAccess("Session.putValue"));
setAttr.Add(All.FindByName("*session.putValue", false));
setAttr.Add(All.FindByMemberAccess("Model.addAttribute"));

result = setAttr;