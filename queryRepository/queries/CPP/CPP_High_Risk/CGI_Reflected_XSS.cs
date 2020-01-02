/*  CGI (Common Gateway Interface) provides opportunities to read files, acquire shell access, and corrupt file systems 
	through environment variables when there is insufficient input validation. 
	This query finds environment inputs that are not sanitized and are influencing outputs
*/ 
if (CGI().Count > 0)
{
	CxList getenv = Find_Environment_Inputs();
	
	CxList sanitize = Find_CGI_Sanitize();
	
	CxList outputs = Find_Outputs();
		
	result = getenv.InfluencingOnAndNotSanitized(outputs, sanitize);
}