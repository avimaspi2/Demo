CxList deadCode = Find_Dead_Code_Contents();

CxList getWriter = All.FindByMemberAccess("HttpServletResponse.getWriter");
getWriter.Add(All.FindByName("*response.getWriter"));  
getWriter.Add(All.FindByName("*Response.getWriter"));

getWriter -= getWriter.GetMembersOfTarget().GetTargetOfMembers();

CxList getFromSystem = All.FindByMemberAccess("System.getenv");

CxList inputs = All.NewCxList();
inputs.Add(getWriter);
inputs.Add(getFromSystem);

CxList interactiveOutputs = Find_Interactive_Outputs();

CxList outputs = interactiveOutputs.FindByShortName("print*");
outputs.Add(interactiveOutputs.FindByShortName("write*"));

CxList sanitize = Find_Integers();
sanitize.Add(deadCode);

result = outputs.InfluencedByAndNotSanitized(inputs, sanitize);