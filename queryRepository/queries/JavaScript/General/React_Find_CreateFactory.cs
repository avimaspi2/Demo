CxList reactReferences = React_Find_References();
CxList reactFirstMember = reactReferences.GetMembersOfTarget();

result = reactFirstMember.FindByShortName("createFactory");