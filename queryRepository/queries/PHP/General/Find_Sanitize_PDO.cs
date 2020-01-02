// there is 2 cases of Execute being safe:
// 1. There is using of bindParam() on statement we execute() without params
// 2. There is parameters trnasferes to the Execute 
CxList methods = Find_Methods();
CxList unkRefs = Find_UnknownReferences();
//find all Execute methods as base to look there
CxList execute = methods.FindByShortName("execute");

// case 1 // find prepared statments with "bindParam" - this is actually the sanitation:
//find all membersAccess of that statments with "execute"
CxList prepStatMemAcc = unkRefs.FindAllReferences(execute.GetTargetOfMembers()).GetMembersOfTarget();
//find bindParam memberAccess from PreparedStatment
CxList bindMethods = prepStatMemAcc.FindByShortName("bindParam");
//take only PreparedStatment with sanitationby bindParam
CxList sanitizedPrepStat = unkRefs.FindAllReferences(bindMethods.GetTargetOfMembers());
// mark all Execute comes from them
result = sanitizedPrepStat.GetMembersOfTarget().FindByShortName("execute");
result.Add(bindMethods);

// case 2 // Now add all Execute that have parametes
CxList allParams = Find_Params();
CxList paramsOfExecute = allParams.GetParameters(execute, 0);
result.Add(execute.FindByParameters(paramsOfExecute));

CxList pdoTarget = unkRefs.FindByType("PDO", false);
CxList pdoMembersSanitizers = pdoTarget.GetMembersOfTarget().FindByShortName("quote");

result.Add(pdoMembersSanitizers);