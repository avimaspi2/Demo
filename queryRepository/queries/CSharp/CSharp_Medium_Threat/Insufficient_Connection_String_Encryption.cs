/// <summary>
/// Checks for insufficient connection string encryption 
/// </summary>

CxList connectionStringsValues = Find_Connection_String_Value();
CxList connectionStringsWithConcatValues = Find_Connection_String_Concat_Value();

// Builds the regex filters
var matchesEncryptRegex = new Regex(@"(Encrypt)=(?<val>True)", RegexOptions.IgnoreCase);
var matchesTCRegex = new Regex(@"(Trusted_Connection)=(?<val>False)", RegexOptions.IgnoreCase);
var matchesTCExistRegex = new Regex(@"Trusted_Connection=", RegexOptions.IgnoreCase);

CxList safeConnectionStrings = All.NewCxList();

CxList strings = Find_Strings().GetByAncs(connectionStringsWithConcatValues);

foreach(CxList concatValue in connectionStringsWithConcatValues)
{
	bool matchesEncryptTrue = false;
	bool matchesTCFalse = false;
	bool matchesTCExist = false;
	bool hasUserId = false;
	bool hasPassword = false;
	
	foreach(CxList str in strings.GetByAncs(concatValue))
	{
		var connection = str.GetName();
		
		matchesEncryptTrue |= matchesEncryptRegex.IsMatch(connection);
		matchesTCFalse |= matchesTCRegex.IsMatch(connection);
		matchesTCExist |= matchesTCExistRegex.IsMatch(connection);
		hasUserId |= connection.Contains("User ID=");
		hasPassword |= connection.Contains("Password=");
	}
	
	bool hasCredentials = hasUserId || hasPassword;
	
	if((matchesEncryptTrue && matchesTCFalse) ||
	    (matchesEncryptTrue && !matchesTCExist) ||
		hasCredentials == false)
	{
		safeConnectionStrings.Add(concatValue);
	}
}

result.Add(connectionStringsWithConcatValues);

// Checks if the connection strings are safe.
foreach(CxList element in connectionStringsValues) 
{
	string connection = element.GetName();
	bool matchesEncryptTrue = matchesEncryptRegex.IsMatch(connection);
	bool matchesTCFalse = matchesTCRegex.IsMatch(connection);
	bool matchesTCExist = matchesTCExistRegex.IsMatch(connection);
	bool hasUserId = connection.Contains("User ID=");
	bool hasPassword = connection.Contains("Password=");
	bool hasCredentials = hasUserId || hasPassword;
	
	if((matchesEncryptTrue && matchesTCFalse) ||
		(matchesEncryptTrue && !matchesTCExist) ||
		hasCredentials == false)
	{
		safeConnectionStrings.Add(element);
	}
}

result.Add(connectionStringsValues);
result -= safeConnectionStrings;