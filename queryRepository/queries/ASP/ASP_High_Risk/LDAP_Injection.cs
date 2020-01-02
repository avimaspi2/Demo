CxList inputs = Find_Interactive_Inputs();
CxList methods = Find_Methods();

CxList sanitize = Find_Integers() + methods.FindByShortName("replace");

CxList de = Find_LDAP();
result = de.InfluencedByAndNotSanitized(inputs, sanitize);
result -= result.DataInfluencedBy(result);