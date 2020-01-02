CxList ldap = All.FindAllReferences(Find_Object("IPWorksASP.LDAP"));
ldap.Add(ldap.GetMembersOfTarget());
result = ldap;