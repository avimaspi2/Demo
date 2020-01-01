CxList inputs = Find_Interactive_Inputs();

CxList exec = Find_Command_Execution();
CxList sanitize = Find_Command_Injection_Sanitize();

result = exec.InfluencedByAndNotSanitized(inputs, sanitize);

// For cases where StartInfo getter is used and then it is updated with data from input.
CxList accessToStartInfo = All.FindByMemberAccess("Process.StartInfo");
CxList startInfo = accessToStartInfo.GetAssignee();
CxList startInfoRefs = All.FindAllReferences(startInfo);
CxList startInfoRefsInfluencedByInput = startInfoRefs.InfluencedByAndNotSanitized(inputs, sanitize);
CxList startInfoInfByInput = startInfoRefs.FindAllReferences(startInfoRefsInfluencedByInput) * startInfo;
CxList processInfByInput = startInfoInfByInput.GetAssigner().GetTargetOfMembers();
CxList processInfByInputRefs = All.FindAllReferences(processInfByInput);
CxList processInfByInputRefsExec = processInfByInputRefs.GetMembersOfTarget() * exec;

result.Add(startInfoRefsInfluencedByInput.ConcatenateAllPaths(processInfByInputRefsExec));