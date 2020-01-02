/*
Some cases:
----------
Password in connection string
	a. Set as value under appSettings (older .Net)
	b. Set as value under connectionStrings (newer .Net)
	c. In a text under ConnectionStrings
Password set in the config file
	a. A key named "password" (or similar) is defined (and set) in the system
	b. A value is set to an element named "Password"
*/

/// General assignments

// Get all config files
CxList config = Find_Web_Config();
config.Add(All.FindByFileName("*app.config"));
// Get all fils in config files
CxList strings = config.FindByType(typeof(StringLiteral));
// Find all conditions in a config file
CxList conditions = config * Find_Conditions();
// Find all password string in config files
CxList passwordString = Find_Password_Strings() * config;
// Find all "key" set in config files
CxList key = config.FindByShortName("KEY");

CxList passwordInConfig = config * Find_All_Passwords();

///Password set in the config file
// a. A key named "password" (or similar) is defined (and set) in the system
CxList passwordKey = passwordString.FindByFathers(key.GetFathers());

// b. A value is set to an element named "Password"
CxList configMembers = config.FindByType(typeof(MemberAccess));

CxList passwordName = All.NewCxList();
passwordName.Add(passwordInConfig);
passwordName.Add(configMembers.FindByShortName("*PASS"));

passwordName = passwordName.DataInfluencedBy(strings);

// Get all results
result.Add(passwordInConfig);
result.Add(passwordKey);
result.Add(passwordName);

// Remove results under SecureAppSettings
CxList secureAppSettings = conditions.FindByShortName("SECUREAPPSETTINGS");
secureAppSettings = secureAppSettings.GetAncOfType(typeof(IfStmt));
result -= result.GetByAncs(secureAppSettings);