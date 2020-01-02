// Find the string literals containig "password"
CxList psw = Find_Password_Strings();

// Find all the connections
CxList con = Find_DB_Connection();

// Add connection strings that contain a password in their initialization
CxList getConnection = Find_Methods().FindByMemberAccess("DriverManager.getConnection")
	+ Find_Methods().FindByMemberAccess("MySQL_Driver.connect")
	+ Find_Methods().FindByMemberAccess("Connection.connect");

CxList connectionStrings = Find_Strings().GetParameters(getConnection, 2);

// Return the "passworded" strings influencing on the connection.
// ...and the string passwords in connection strings.
// Notice, that only parameters should affect the connections, as they are defined above.
result = con.DataInfluencedBy(psw + connectionStrings) + connectionStrings;