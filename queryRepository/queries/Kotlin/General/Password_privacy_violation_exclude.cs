// Exclusion List of passwords
CxList passwords = Password_privacy_violation_list();

//remove constant passwords of type password_XXX
CxList constPass = passwords.FindByType(typeof(ConstantDeclStmt));
CxList declPass = passwords.FindByType(typeof(Declarator));
CxList allConstantPass = declPass.FindByShortName(constPass);
allConstantPass.Add(constPass);

List<string> allConstantPassNames = new List<string>{
		"password_*","psw_*","pwd_*","pass_*"};

result = allConstantPass.FindByShortNames(allConstantPassNames, false);

// Removes booleans such as "passes, passed, pass, hasPassed" and so on...
// remove references of passwordExpireDate or PASSWORD_EXPIRATION_PERIOD
List<string> passwordsMethodsNames = new List<string>{
		"*Expiration*","*expiration*","*Expire*","*expire*","*Date*"};

result.Add(passwords.FindByShortNames(passwordsMethodsNames));

result.Add(All.FindByTypes(new string []{"Date","bool"}));
result.Add(passwords.FindByShortName("*passthrough*", false));