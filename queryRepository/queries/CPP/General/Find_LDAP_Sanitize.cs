//Find all LDAP sanitized

//Find all the function.
CxList methods = Find_Methods();

//Find all the LDAP methods that their parameters are sanitized

//Find all the methods that include replace in thier name
CxList methodsWithReplace = methods.FindByShortName("*replace*");

CxList ldapMethods = All.NewCxList();
ldapMethods = methodsWithReplace.FindByShortName("replace_all");
ldapMethods.Add(methodsWithReplace.FindByShortName("ireplace_all"));
ldapMethods.Add(methodsWithReplace.FindByShortName("replace_range"));
ldapMethods.Add(methodsWithReplace.FindByShortName("replace_first"));
ldapMethods.Add(methodsWithReplace.FindByShortName("ireplace_first"));
ldapMethods.Add(methodsWithReplace.FindByShortName("replace_last"));
ldapMethods.Add(methodsWithReplace.FindByShortName("ireplace_last"));
ldapMethods.Add(methodsWithReplace.FindByShortName("replace_head"));
ldapMethods.Add(methodsWithReplace.FindByShortName("replace_tail"));
ldapMethods.Add(methods.FindByShortName("split"));
ldapMethods.Add(methodsWithReplace.FindByShortName("replace_regex"));
ldapMethods.Add(methodsWithReplace.FindByShortName("replace_all_regex"));
ldapMethods.Add(methods.FindByShortName("erase_regex"));
ldapMethods.Add(methods.FindByShortName("erase_all_regex"));
CxList parameters = All.GetParameters(ldapMethods,0);

result = parameters;
result.Add(All.FindDefinition(parameters));

//Find all the LDAP sanitize that their return value is sanitized

//Find all the methods that ends with _copy
CxList methodsWithCopy = methods.FindByShortName("*_copy");
//Find all the methods that include replace and ends with _copy
CxList methodsWithReplaceAndCopy = methodsWithCopy * methodsWithReplace;

result.Add(methodsWithReplace.FindByShortName("regex_replace"));
result.Add(methodsWithReplace.FindByShortName("replace_all"));
result.Add(methodsWithReplaceAndCopy.FindByShortName("replace_all_copy"));
result.Add(methodsWithReplaceAndCopy.FindByShortName("ireplace_all_copy"));
result.Add(methodsWithReplaceAndCopy.FindByShortName("replace_range_copy"));
result.Add(methodsWithReplaceAndCopy.FindByShortName("ireplace_first_copy"));
result.Add(methodsWithReplaceAndCopy.FindByShortName("replace_first_copy"));
result.Add(methodsWithReplaceAndCopy.FindByShortName("replace_last_copy"));
result.Add(methodsWithReplaceAndCopy.FindByShortName("replace_head_copy"));
result.Add(methodsWithReplaceAndCopy.FindByShortName("ireplace_last_copy"));
result.Add(methodsWithReplaceAndCopy.FindByShortName("replace_tail_copy"));
result.Add(methodsWithReplaceAndCopy.FindByShortName("replace_regex_copy"));
result.Add(methodsWithReplaceAndCopy.FindByShortName("replace_all_regex_copy"));
result.Add(methodsWithCopy.FindByShortName("erase_regex_copy"));
result.Add(methodsWithCopy.FindByShortName("erase_all_regex_copy"));
result.Add(All.FindByMemberAccess("char*.replace").GetTargetOfMembers()); //in c
result.Add(All.FindByMemberAccess("char*.erase").GetTargetOfMembers());
result.Add(All.FindByMemberAccess("string.replace").GetTargetOfMembers()); //in cpp
result.Add(All.FindByMemberAccess("string.erase").GetTargetOfMembers());

CxList isalnum = methods.FindByShortName("isalnum"); 
result.Add(Find_Unknown_References().GetByAncs(isalnum));
result.Add(All.GetParameters(isalnum));