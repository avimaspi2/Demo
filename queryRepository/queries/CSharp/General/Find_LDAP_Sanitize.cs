result.Add(Find_Integers());
result.Add(All.FindByName("*Regex.Replace"));
result.Add(All.FindByMemberAccess("Regex.Replace"));
result.Add(Find_Replace());

// Support for LDAP encoders in Microsoft AntiXSS Library.
result.Add(All.FindByMemberAccess("Encoder.LdapEncode"));
result.Add(All.FindByMemberAccess("Encoder.LdapFilterEncode"));
result.Add(All.FindByMemberAccess("Encoder.LdapDistinguishedNameEncode"));