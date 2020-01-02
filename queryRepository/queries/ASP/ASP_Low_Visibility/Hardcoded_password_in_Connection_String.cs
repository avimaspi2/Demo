// 1. Find the members 'ConnectionString' and 'Open' from the class ADODB
CxList connections = Find_Member_With_Target("ADODB.Connection", "ConnectionString");
connections.Add(Find_Member_With_Target("ADODB.Connection", "Open"));

// 2. Find all hardcoded passwords
CxList hardcodedPasswords = Find_Password_Strings();

// 3. Remove the hardcoded passwords that are parameters of the method Request.Form
hardcodedPasswords -= hardcodedPasswords.GetParameters(All.FindByMemberAccess("request.form", false));

// 4. Find hardcoded passwords used in a connection
result = connections.DataInfluencedBy(hardcodedPasswords);