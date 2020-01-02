CxList sanitizers = All.NewCxList();

sanitizers.Add(Find_Sanitize());
sanitizers.Add(Find_LDAP_Sanitize());

result = sanitizers;