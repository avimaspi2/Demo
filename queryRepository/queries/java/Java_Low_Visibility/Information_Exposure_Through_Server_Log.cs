CxList methods = Find_Methods();
CxList deadCode = Find_Dead_Code_Contents();

CxList getSession = All.FindByMemberAccess("HttpServletRequest.getSession");
getSession.Add(All.FindByName("*response.getSession"));
getSession.Add(All.FindByName("*Response.getSession"));

CxList inputs = getSession.GetMembersOfTarget().FindByShortName("getId");
inputs.Add(All.FindByMemberAccess("HttpSession.getId"));

CxList outputs = methods.FindByName("log", false);

CxList sanitize = Find_Integers();
sanitize.Add(deadCode);

result = outputs.InfluencedByAndNotSanitized(inputs, sanitize);