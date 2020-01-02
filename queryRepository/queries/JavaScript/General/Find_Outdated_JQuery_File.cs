//This query finds all JQuery files that their version is set to lower than 2.2
//Or, if given a parameter, lower than its value.
string latestVersion = "2.2";
string versionRegex = @"jQuery Typescript Library v";
bool secondParam = false;

try
{
	if (param.Length != 0)
	{
		latestVersion = param[0] as string;
		if (latestVersion == null)
		{
			throw new Exception("Find_Outdated_JQuery_File: first param is not a string");
		}
		
		secondParam = param.Length > 1;
		if (secondParam) 
		{
			versionRegex = param[1] as string;
			if (versionRegex == null)
			{
				cxLog.WriteDebugMessage("Find_Outdated_JQuery_File: second param is not a string");
				versionRegex = @"jQuery Typescript Library v";
			}
		}
	}

	string regexTest = @"(\d+\.\d+(\.\d+)?)";

	// search in comments only if no search pattern for the file name is given.
	CxList reg = All.FindByRegexExt(versionRegex + regexTest, "*.*", !secondParam, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

	result = Semantic_Version_In_Range(reg, "0.0.0", latestVersion, versionRegex);
}
catch (Exception exc)
{
	cxLog.WriteDebugMessage(exc.Message);
}