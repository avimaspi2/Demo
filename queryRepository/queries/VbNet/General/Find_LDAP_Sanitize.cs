result = Find_Integers();
result.Add(All.FindByName("*Regex.Replace", false));
result.Add(All.FindByMemberAccess("Regex.Replace", false));
result.Add(Find_Replace());

// Support for LDAP encoders in Microsoft AntiXSS Library.
result.Add(All.FindByMemberAccess("Encoder.LdapEncode", false));
result.Add(All.FindByMemberAccess("Encoder.LdapFilterEncode", false));
result.Add(All.FindByMemberAccess("Encoder.LdapDistinguishedNameEncode", false));