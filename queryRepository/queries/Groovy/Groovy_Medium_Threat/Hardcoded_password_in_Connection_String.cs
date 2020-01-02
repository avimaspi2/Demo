// Find the string literals containig "password"
CxList psw = Find_Password_Strings();

// Find creation of connections or connection strings, influenced by password
CxList createExpressions = Find_Object_Create();
CxList openConnection = createExpressions.FindByShortName("*Connection") +
	All.FindByMemberAccess("Connection.ConnectionString");

result = openConnection.DataInfluencedBy(psw);


// Find connection strings that contain a password in their initialization
CxList getConnection = All.FindByMemberAccess("DriverManager.getConnection");
CxList connectionStrings = All.GetParameters(getConnection, 2).FindByType(typeof(StringLiteral));

// Add the path from the string/parameter to its method
result.Add(getConnection.DataInfluencedBy(connectionStrings));

// Find connection strings that contain a password in their initialization in groovy SQL API
CxList sqlNewInstance = All.FindByMemberAccess("Sql.newInstance");
CxList sqlConnectionStrings = All.GetParameters(sqlNewInstance, 2).FindByType(typeof(StringLiteral));
// Add the path from the string/parameter to its method
result.Add(sqlNewInstance.DataInfluencedBy(sqlConnectionStrings));



// Find Java-specific password in connection string
CxList DManagerDataSource = All.FindByMemberAccess("DriverManagerDataSource.setPassword");
CxList pass = All.GetParameters(DManagerDataSource, 0).FindByType(typeof(StringLiteral));

// Add the path from the string/parameter to its method
result.Add(DManagerDataSource.DataInfluencedBy(pass));