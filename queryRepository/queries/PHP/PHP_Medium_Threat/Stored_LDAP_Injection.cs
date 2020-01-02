CxList methods = Find_Methods();
CxList db = Find_DB_Out() + Find_Read();

CxList sanitized = Find_General_Sanitize() + Find_LDAP_Replace();

CxList ldap_find_methods = methods.FindByShortNames(new List<string>{ "ldap_list", "ldap_read", "ldap_search" });
CxList filter_params = All.FindByFathers(All.GetParameters(ldap_find_methods, 2));

result = filter_params.InfluencedByAndNotSanitized(db, sanitized);
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);