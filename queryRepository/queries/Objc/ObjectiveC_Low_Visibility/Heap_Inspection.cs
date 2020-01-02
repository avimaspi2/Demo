CxList methods = Find_Methods();

List<string> methodsNames = new List<string> {"realloc","fork","vfork"};
CxList badForSecurity = methods.FindByShortNames(methodsNames);

CxList personal = Find_Personal_Info();

result = badForSecurity.DataInfluencedBy(personal);