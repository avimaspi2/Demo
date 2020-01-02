CxList reactReferences = React_Find_References();
CxList reactFirstMember = reactReferences.GetMembersOfTarget();

CxList createElementMethods = reactFirstMember.FindByShortName("createElement");

result = createElementMethods;