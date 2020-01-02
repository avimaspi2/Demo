CxList methods = Find_Methods();

CxList inputs = Find_Interactive_Inputs();

CxList sanitized = Find_General_Sanitize() + Find_LDAP_Replace();


CxList ldap_find_methods = methods.FindByShortNames(new List<string>(){ "ldap_list", "ldap_read", "ldap_search"});

CxList filter_params = All.GetParameters(ldap_find_methods, 2);

result = filter_params.InfluencedByAndNotSanitized(inputs, sanitized);
result.Add(filter_params * inputs);