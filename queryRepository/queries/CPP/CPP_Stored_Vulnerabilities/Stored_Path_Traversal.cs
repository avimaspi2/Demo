if (CGI().Count > 0) //web application (CGI)
{
	CxList inputs = Find_Read() + Find_DB();
	CxList methodInvokes = Find_Methods();
	CxList files = Find_Open_Files_Methods() + 
		methodInvokes.FindByShortName("unlink"); //to open a file
	CxList firstParam = All.GetParameters(files, 0);//only the first param of the method counts
	CxList sanitize = Find_Sanitize();

	result = inputs.InfluencingOnAndNotSanitized(firstParam, sanitize);
}