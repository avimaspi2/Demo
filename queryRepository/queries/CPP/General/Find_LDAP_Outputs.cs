//Find all the LDAP Outputs

//Find all the function
CxList methods = Find_Methods();

//Fillter the functions to only LDAP functions
result = methods.FindByShortName("ldap_add_ext_s");
result.Add(methods.FindByShortName("ldap_add_ext"));
result.Add(methods.FindByShortName("ldap_add_s"));
result.Add(methods.FindByShortName("ldap_add"));
result.Add(methods.FindByShortName("ldap_compare_ext_s"));
result.Add(methods.FindByShortName("ldap_compare_ext"));
result.Add(methods.FindByShortName("ldap_compare_s"));
result.Add(methods.FindByShortName("ldap_compare"));
result.Add(methods.FindByShortName("ldap_delete_ext_s"));
result.Add(methods.FindByShortName("ldap_delete_ext"));
result.Add(methods.FindByShortName("ldap_delete_s"));
result.Add(methods.FindByShortName("ldap_delete"));
result.Add(methods.FindByShortName("ldap_modify_ext_s"));
result.Add(methods.FindByShortName("ldap_modify_ext"));
result.Add(methods.FindByShortName("ldap_modify_s"));
result.Add(methods.FindByShortName("ldap_modify"));
result.Add(methods.FindByShortName("ldap_modrdn_s"));
result.Add(methods.FindByShortName("ldap_modrdn"));
result.Add(methods.FindByShortName("ldap_modrdn2_s"));
result.Add(methods.FindByShortName("ldap_modrdn2"));
result.Add(methods.FindByShortName("ldap_rename_ext"));
result.Add(methods.FindByShortName("ldap_rename_ext_s"));
result.Add(methods.FindByShortName("ldap_search_ext_s"));
result.Add(methods.FindByShortName("ldap_search_ext"));
result.Add(methods.FindByShortName("ldap_search_s"));
result.Add(methods.FindByShortName("ldap_search_init_page"));
result.Add(methods.FindByShortName("ldap_search"));
result.Add(methods.FindByShortName("ldap_search_st"));