/*
Some cases:
//////////
1. Password in connection string
	a. Set as value under appSettings (older .Net)
	b. Set as value under connectionStrings (newer .Net)
	c. In a text under ConnectionStrings
2. Password set in the config file
	a. A key named "password" (or similar) is defined (and set) in the system
	b. A value is set to an element named "Password"
*/

/// General assignments
if(AllMembers.All.FindByLanguage("CSharp").Count == 0)
{

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
	CxList key = config.FindByShortName("key", false);


	/// 1. Connection strings

	// Get all passwords that are set in a connection string
	CxList passwordEquals = strings.FindByShortName("*password=*", false);
	passwordEquals.Add(strings.FindByShortName("*password =*", false));
	passwordEquals.Add(strings.FindByShortName("*pwd=*", false));
	passwordEquals.Add(strings.FindByShortName("*pwd =*", false));

	// Get the connection strings elements in the config files
	CxList connectionStrings = conditions.FindByShortName("*connectionstrings", false);
	connectionStrings.Add(conditions.FindByShortName("appsettings", false));
	connectionStrings = connectionStrings.GetAncOfType(typeof(IfStmt));

	// Find passwords in connection strings
	CxList passwordInConnectionString = passwordEquals.GetByAncs(connectionStrings);


	/// 2. Password set in the config file

	// 2a. A key named "password" (or similar) is defined (and set) in the system
	CxList passwordKey = passwordString.FindByFathers(key.GetFathers());

	// 2b. A value is set to an element named "Password"
	CxList configMembers = config.FindByType(typeof(MemberAccess));
	CxList passwordName = configMembers * Find_All_Passwords();
	passwordName.Add(configMembers.FindByShortName("*pass", false));

	passwordName = passwordName.DataInfluencedBy(strings);

	// Get all results
	result.Add(passwordInConnectionString);
	result.Add(passwordKey);
	result.Add(passwordName);


	// Remove results under SecureAppSettings
	CxList secureAppSettings = conditions.FindByShortName("secureappsettings", false);
	secureAppSettings = secureAppSettings.GetAncOfType(typeof(IfStmt));
	result -= result.GetByAncs(secureAppSettings);
}