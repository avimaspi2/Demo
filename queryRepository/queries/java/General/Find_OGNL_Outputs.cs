CxList methods = Find_Methods();

//ActionContext
CxList getContext = methods.FindByMemberAccess("ActionContext.getContext");
getContext.Add(All.FindAllReferences(getContext.GetAssignee()));
CxList getValueStack = getContext.GetMembersOfTarget().FindByShortName("getValueStack");
getValueStack.Add(All.FindAllReferences(getValueStack.GetAssignee()));
CxList findText = methods.FindByMemberAccess("LocalizedTextUtil.findText");

result.Add(getValueStack.GetMembersOfTarget().FindByShortName("findValue"));
result.Add(getValueStack.GetMembersOfTarget().FindByShortName("findString"));

CxList setValueParameter = getValueStack.GetMembersOfTarget().FindByShortName("setValue");
setValueParameter.Add(getValueStack.GetMembersOfTarget().FindByShortName("setParameter"));
result.Add(All.GetParameters(setValueParameter, 0));

result.Add(All.GetParameters(findText, 1));

//ActionSupport
CxList getFormatted = methods.FindByMemberAccess("ActionSupport.getFormatted");
getFormatted -= All.FindAllReferences(All.FindDefinition(getFormatted));
result.Add(All.GetParameters(getFormatted, 1));

//OGNL
CxList parseExpr = methods.FindByMemberAccess("Ognl.parseExpression");
result.Add(methods.FindByMemberAccess("Ognl.getValue").DataInfluencedBy(parseExpr));

// Requests 
CxList request = All.NewCxList();
request.Add(methods.FindByName("request.getAttribute"));
request.Add(methods.FindByMemberAccess("HttpServletRequest.getAttribute"));
result.Add(All.GetParameters(request, 0));