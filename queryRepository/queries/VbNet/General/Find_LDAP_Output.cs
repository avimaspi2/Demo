List<string> LDAPConstructors = new List<string> {"*DirectoryEntry","LdapConnection"};
CxList LDAP_Object = All.FindByShortNames(LDAPConstructors, false).FindByType(typeof(ObjectCreateExpr));
CxList ds = All.FindByType("*DirectorySearcher", false);

CxList LDAP_Object_Params = All.GetParameters(LDAP_Object);
CxList ldap = ds;
ldap.Add(LDAP_Object_Params);

ldap -= Find_Methods().FindByShortName("Dispose", false).GetTargetOfMembers();

result = ldap;