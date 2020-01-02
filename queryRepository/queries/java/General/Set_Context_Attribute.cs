CxList getContext = All.FindByMemberAccess("request.getServletContext");
getContext.Add(All.FindByName("*request.getServletContext"));
getContext.Add(All.FindByName("*Request.getServletContext"));

CxList setAttr = getContext.GetMembersOfTarget().FindByShortName("setAttribute");
setAttr.Add(All.FindByMemberAccess("Context.setAttribute"));
setAttr.Add(All.FindByName("*context.setAttribute", false));
setAttr.Add(All.FindByName("*context.set", false));
setAttr.Add(All.FindByMemberAccess("context.put"));

result = setAttr;