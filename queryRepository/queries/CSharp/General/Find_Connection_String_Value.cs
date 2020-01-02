/// <summary>
/// This query was built to return all the connection string values except the ones with concat values.
/// </summary>

CxList strings = Find_Strings();
CxList connections = Find_Connection_String();
CxList potentialConnString = strings.FindByShortName("*=*");

// Gets the connection string with the following sintax... connection.ConnectrionString = "...";
CxList connectionStringAssign = connections.GetMembersOfTarget().FindByShortName("ConnectionString").GetAssigner();
result.Add(potentialConnString.DataInfluencingOn(connectionStringAssign));

// Gets the connection string with the following sintax: new SqlConnection("...")
result.Add(All.GetParameters(connections.FindByType(typeof(ObjectCreateExpr))) * potentialConnString);

//Get the connection strings from the *.config files
CxList config = All.FindByFileName("*.config");
CxList connectionStringFormFile = config.FindByName("CONFIGURATION.CONNECTIONSTRINGS.CONNECTIONSTRING").GetAssigner();
result.Add(connectionStringFormFile);

// Removes connection string concat values 
CxList binaryExprAdd = Find_Connection_String_Concat_Value();
CxList binaryExprAddDescendents = binaryExprAdd - binaryExprAdd.FindByFathers(binaryExprAdd);

result -= result.GetByAncs(binaryExprAddDescendents);