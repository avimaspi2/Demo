result = All.FindByType("*__c");

result -= result.GetTargetOfMembers().GetMembersOfTarget();
result.Add(Find_MetaData_Objects());
result.Add(Find_VF_Pages().FindAllReferences(result.GetFathers().FindByType(typeof(MethodDecl))));