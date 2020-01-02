CxList passwords = All_Passwords();
result = passwords.FindByType(typeof(UnknownReference));
result.Add(passwords.FindByType(typeof(ParamDecl)));
result.Add(passwords.FindByType(typeof(Declarator)));
result.Add(passwords.FindByType(typeof(MemberAccess)));