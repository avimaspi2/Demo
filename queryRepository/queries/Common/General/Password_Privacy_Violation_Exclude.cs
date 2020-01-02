// Exclusion List of passwords
CxList passwords = Password_Privacy_Violation_List();

//remove constant passwords of type password_XXX
CxList constPass = passwords.FindByType(typeof(ConstantDeclStmt));
CxList declPass = passwords.FindByType(typeof(Declarator));
CxList allConstantPass = declPass.FindByShortName(constPass);
allConstantPass.Add(constPass);

result = allConstantPass.FindByShortName("password_*", false);
result.Add(allConstantPass.FindByShortName("psw_*", false));
result.Add(allConstantPass.FindByShortName("pwd_*", false));
result.Add(allConstantPass.FindByShortName("pass_*", false));

result.Add(All.FindByType("bool")); // Removes booleans such as "passes, passed, pass, hasPassed" and so on...
// remove references of passwordExpireDate or PASSWORD_EXPIRATION_PERIOD
List<string> passwordExpiration = new List<string>{
		"*Expiration*",
		"*expiration*",
		"*Expire*",
		"*expire*",
		"*Date*"
		};

result.Add(passwords.FindByShortNames(passwordExpiration));
result.Add(passwords.FindByShortName("*passthrough*", false));

result.Add(All.FindByType("Date"));