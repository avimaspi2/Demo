// Exclusion List of passwords
CxList passwords = Password_privacy_violation_list();

//remove constant passwords of type password_XXX
CxList constPass = passwords.FindByType(typeof (ConstantDeclStmt));
CxList declPass = passwords.FindByType(typeof (Declarator));
CxList allConstantPass = declPass.FindByShortName(constPass) + constPass;

result = allConstantPass.FindByShortName("password_*", false);
result.Add(allConstantPass.FindByShortName("psw_*", false));
result.Add(allConstantPass.FindByShortName("pwd_*", false));
result.Add(allConstantPass.FindByShortName("pass_*", false));

result.Add(passwords.FindByType("bool")); // Removes booleans such as "passes, passed, pass, hasPassed" and so on...
// remove references of passwordExpireDate or PASSWORD_EXPIRATION_PERIOD
result.Add(passwords.FindByShortName("*Expiration*"));
result.Add(passwords.FindByShortName("*expiration*"));
result.Add(passwords.FindByShortName("*Expire*"));
result.Add(passwords.FindByShortName("*expire*"));
result.Add(passwords.FindByShortName("*Date*"));
result.Add(passwords.FindByShortName("*passthrough*",false));
result.Add(passwords.FindByType("Date"));
result.Add(passwords.FindByShortName("*passiv*", false));