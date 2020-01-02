//Find methods that establish a connection to a database using a connection string
CxList connection = Find_Methods().FindByShortNames(new List<string>(){"SQLConnect","SqlConnection","SQLDriverConnect"});
	
connection.Add(Find_Methods().FindByMemberAccess("Driver.connect"));
connection.Add(Find_Methods().FindByMemberAccess("Connection.open"));
connection.Add(Find_Methods().FindByMemberAccess("Connection.connect"));
connection.Add(Find_Methods().FindByMemberAccess("CDatabase.Open"));
connection.Add(Find_Methods().FindByMemberAccess("CDatabase.OpenEx"));


//Find inputs
CxList inputs = Find_Read();
inputs.Add(Find_DB());


//User inputs that may change the string that is used to establish a connection to a database
result = connection.InfluencedBy(inputs);