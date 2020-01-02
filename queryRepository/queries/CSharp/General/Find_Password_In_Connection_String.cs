CxList strings = All.FindByType(typeof(StringLiteral));

result = strings.FindByShortNames(new List<string> {
		"*password=*", "*password =*",
		"*pwd=*", "*pwd =*"
		}, false);

//Remove all hardcoded passwords ends with "password=" and "pwd=" for cases like: ConnectionString = "pwd=" + Password.  
foreach (CxList pass in result)
{
	string trimPass = pass.GetName().Trim().ToLower();
	if (trimPass.EndsWith("password=") || trimPass.EndsWith("pwd=") ||
		trimPass.EndsWith("password =") || trimPass.EndsWith("pwd ="))
	{
		result -= pass;
	}	
}