// Find the string literals containig "password"
CxList psw = Find_Password_In_Connection_String();

// Find all the connections
CxList createExpressions = All.FindByType(typeof(ObjectCreateExpr));

CxList openConnection = createExpressions.FindByShortNames(new List<string> {
		"*Connection", "*DataContext"});
openConnection.Add(All.FindByMemberAccess("Connection.ConnectionString"));

// Return the connections influenced by "passworded" strings.
// Notice that since it's an ObjectCreateExpr ("new()"), only its parameters
// can influence it.
result = openConnection.DataInfluencedBy(psw);


// Add connection strings that contain a password in their initialization
CxList getConnection = All.FindByMemberAccess("DriverManager.getConnection");
CxList connectionStrings = getConnection.DataInfluencedBy(All.GetParameters(getConnection, 2).FindByType(typeof(StringLiteral)));

result.Add(connectionStrings);