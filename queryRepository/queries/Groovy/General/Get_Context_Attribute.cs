CxList getContext = 
	All.FindByMemberAccess("request.getServletContext") +
	All.FindByName("*request.getServletContext") +
	All.FindByName("*Request.getServletContext");

CxList getAttr = getContext.GetMembersOfTarget().FindByShortName("getAttribute");
getAttr.Add(All.FindByMemberAccess("Context.getAttribute"));
getAttr.Add(All.FindByName("*context.getAttribute", false));
getAttr.Add(All.FindByName("*context.get", false));
getAttr.Add(All.FindByMemberAccess("context.get"));

result = getAttr;