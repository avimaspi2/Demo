/////////////////////////////////////////////////
//			Find_DB_Connection_Passwords
// Finds all the password that are used to connect to a DB
// Return the password paramter that is passed
// to a connection method that is used to connect to a DB
// For example in MySQL_Driver*.connect method' the scond parameter is the password
/////////////////////////////////////////////////

CxList methods = Find_Methods();
	
CxList MySQL_Driver = All.FindByMemberAccess("MySQL_Driver*.connect");
result.Add(All.GetParameters(MySQL_Driver,2));

CxList Driver = All.FindByMemberAccess("Driver*.connect");
result.Add(All.GetParameters(Driver, 2));

CxList SQLConnect = methods.FindByShortName("SQLConnect");
result.Add(All.GetParameters(SQLConnect, 5));

CxList Environment = All.FindByMemberAccess("Environment*.createConnection");
result.Add(All.GetParameters(Environment, 1));